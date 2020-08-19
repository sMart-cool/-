using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Tetris
{
    public delegate void KeyDownEventHander(ConsoleKey key);
    public delegate void PaintEventHander(Type type, Element element);
    public delegate void RestructWallEventHander();

    //枚举，描述如何进行UI绘图
    public enum Type
    {
        //将方块组成的墙画出
        ALLWallTetris,
        //画出一个方块
        OneTetris,
        //清除由全部方块组成的墙
        ClearAllWllTetris
    }

    //游戏过程类，负责运行整个游戏的类
    public class GameProcess
    {
        //按键事件
        public event KeyDownEventHander KeyDown;

        //绘制事件
        public event PaintEventHander Paint;

        //重构整个俄罗斯方块所构成的墙的事件
        public event RestructWallEventHander RestructWall;

        //定义一个游戏数据类，里面包含了游戏运行的信息
        GameData gameData;

        //定义一个线程，键盘按下线程，监听用户输入
        Thread keyDownThread;

        //定义一个定时器timer
        System.Timers.Timer timer;

        //开始运行游戏
        public void StartGame()
        {
            InitGameData();
            InitGameUI();
            keyDownThread = new Thread(KeyDownThread);//线程
            Paint += new PaintEventHander(UIPaint);
            RestructWall += new RestructWallEventHander(RestructWallEvent);

            KeyDown += new KeyDownEventHander(KeyDownEvent);
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            keyDownThread.Start();
            timer.Start();

        }

        //用于固定一段时间刷新游戏画面timer事件
        private void timer_Elapsed(object Source, ElapsedEventArgs e)
        {
            //每隔一段时间就触发一下KeyDown事件，传入的按下键的参数
            KeyDown(ConsoleKey.DownArrow);
        }

        //一个用于接收键盘输入的线程
        private void KeyDownThread()
        {
            while (true)
            {
                //子线程监听键盘按下，只要按下就触发事件
                KeyDown(Console.ReadKey(true).Key);//触发KeyDown事件
            }
        }

        //初始化游戏数据
        private void InitGameData()
        {
            gameData = new GameData();
            gameData.CurrentTetris = new Element();
            gameData.CurrentTetris.GenerateTetris(gameData.RunningRect.X + gameData.RunningRect.Width / 2 - 2, gameData.RunningRect.Y);
            gameData.NextTetris = new Element();
            gameData.NextTetris.GenerateTetris(gameData.ShowRect.X+1,gameData.ShowRect.Y+gameData.ShowRect.Height/2-2);
        }
        
        //初始化游戏界面  
        private void InitGameUI()
        {
            //设置光标位置
            Console.SetCursorPosition(gameData.RunningRect.X-1,gameData.RunningRect.Y-2);
            Console.WriteLine("提示：输入Q退出，R重玩           ");
            Console.SetCursorPosition(gameData.RunningRect.X-1,gameData.RunningRect.Y-1);
            //Console.WriteLine("******************");//33个*头部
            string Str = null;
            for (int i = 0; i < gameData.RunningRect.Width+gameData.ShowRect.Width+3; i++)
            {
                Str += "*";
            }
            Console.WriteLine(Str);
            for (int y = gameData.RunningRect.Y; y < gameData.RunningRect.Y+gameData.RunningRect.Height; y++)
            {
                Console.SetCursorPosition(gameData.RunningRect.X - 1, y);

                //第一个为20个空格，游戏界面，第二个为10个空格下一个图形显示界面
                string str = "*";
                for (int i = 0; i < gameData.RunningRect.Width; i++)
                {
                    str += " ";
                }
                str += "*";
                for (int i = 0; i < gameData.ShowRect.Width; i++)
                {
                    str += " ";
                }
                str += "*";
                Console.WriteLine(str);
            }

            //输出33个*,界面底部
            Str = null;
            Console.SetCursorPosition(gameData.RunningRect.X - 1, gameData.RunningRect.Y + gameData.RunningRect.Height);
            for (int i = 0; i < gameData.RunningRect.Width+gameData.ShowRect.Width+3; i++)
            {
                Str += "*";
            }
            Console.WriteLine(Str);
            //设置显示得分位置
            Console.SetCursorPosition(gameData.ShowRect.X + 1, gameData.ShowRect.Y + gameData.ShowRect.Height - 3);
            Console.WriteLine("Score:{0}",gameData.Score);
            ShowOneTetris(gameData.CurrentTetris);
            ShowOneTetris(gameData.NextTetris);
        }

        //根据4*4的数组将一个俄罗斯方块显示出来
        private void ShowOneTetris(Element element)
        {
            for (int i = 0; i <= element.Tetris.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= element.Tetris.GetUpperBound(1); j++)
                {                 //因为'■'占用两个字符，所以横坐标每次加2*i
                    if (element.Tetris[i,j])
                    {
                        Console.SetCursorPosition(element.Coordinate.X+2*j,element.Coordinate.Y+i);
                        Console.WriteLine(Element.Square);
                    }
                }
            }
        }


        //根据输入的不同的键响应不同的操作
        private void KeyDownEvent(ConsoleKey key)
        {
            gameData.CurrentTetris.CopyTo(gameData.FormerElement);
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    gameData.CurrentTetris.TransformTetris();//下面判断是否能翻转，不能翻转则把上次的状态拷贝回来
                    for (int i = 0; i <=gameData.CurrentTetris.Tetris.GetUpperBound(0); i++)
                    {
                        for (int j = 0; j <= gameData.CurrentTetris.Tetris.GetUpperBound(1); j++)
                        {
                            if (gameData.CurrentTetris.Tetris[i,j])
                            {             //因为'■'占用两个字符，所以横坐标每次加2*i,列数是实际值的1/2
                                if (  (gameData.CurrentTetris.Coordinate.Y+i-gameData.RunningRect.Y)>=GameData.Row
                                    ||(gameData.CurrentTetris.Coordinate.X+j*2-gameData.RunningRect.X)/2>=GameData.Col
                                    || (gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y) < 0
                                    || (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2 < 0
                                    )
                                {
                                    gameData.FormerElement.CopyTo(gameData.CurrentTetris);
                                    return;
                                }
                                //如果屏幕这个位置已经有方块为true了也不能变形
                                else if (gameData.Screen[gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y, (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2])
                                {
                                    gameData.FormerElement.CopyTo(gameData.CurrentTetris);
                                    return;
                                }
                            }
                        }
                    }
                    break;

                case ConsoleKey.DownArrow:
                    for (int j = 0; j <=gameData.CurrentTetris.Tetris.GetUpperBound(1) ; j++)
                    {
                        for (int i = gameData.CurrentTetris.Tetris.GetUpperBound(0); i >=0; i--)
                        {
                            if (gameData.CurrentTetris.Tetris[i,j])
                            {       //如果方块已经倒了最底层或方块下面已有方块，则方块停留在此处，继续下一块方块的出现，同时判断游戏是否结束
                                if ((gameData.CurrentTetris.Coordinate.Y+i)==(gameData.RunningRect.Y+gameData.RunningRect.Height-1)
                                    || gameData.Screen[(gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y + 1), (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2])
                                {
                                    for (j = 0; j <= gameData.CurrentTetris.Tetris.GetUpperBound(1); j++)
                                    {
                                        for (i = gameData.CurrentTetris.Tetris.GetUpperBound(0); i >= 0; i--)
                                        {
                                            if (gameData.CurrentTetris.Tetris[i, j])
                                            {
                                                gameData.Screen[(gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y), (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2] = true;
                                            }
                                        }
                                    }
                                            if (IsGameOver())
                                            {
                                                timer.Stop();
                                                //将这个方块显示出来让玩家看到。才知道游戏结束
                                                ShowOneTetris(gameData.CurrentTetris);
                                                Console.SetCursorPosition(gameData.RunningRect.X - 6, gameData.RunningRect.Y + gameData.RunningRect.Height + 1);
                                                Console.WriteLine("游戏结束，你的分数是：{0}，输入Q退出，R重玩", gameData.Score);

                                                while (true)
                                                {
                                                    switch (Console.ReadKey(true).Key)
                                                    {
                                                        case ConsoleKey.Q:
                                                            keyDownThread.Abort();//终止线程
                                                            return;
                                                        case ConsoleKey.R:
                                                            ReStartGame();
                                                            return;

                                                    }
                                                }
                                            }
                                            gameData.NextTetris.CopyTo(gameData.CurrentTetris);
                                            gameData.CurrentTetris.Coordinate.X = gameData.RunningRect.X + gameData.RunningRect.Width / 2 - 2;
                                            gameData.CurrentTetris.Coordinate.Y = gameData.RunningRect.Y;
                                            gameData.NextTetris.CopyTo(gameData.FormerElement);
                                            gameData.NextTetris.GenerateTetris(gameData.ShowRect.X + 1, gameData.ShowRect.Y + gameData.ShowRect.Height / 2 - 2);
                                            Paint(Type.OneTetris, gameData.NextTetris);
                                            RestructWall();
                                            return;
                                        
                                    
                                }
                                else
                                    break;
                            }
                        }
                    }
                    gameData.CurrentTetris.Coordinate.Y++;
                    break;
                case ConsoleKey.LeftArrow:
                    for (int i = 0; i <= gameData.CurrentTetris.Tetris.GetUpperBound(0); i++)
                    {
                        for (int j = 0; j <= gameData.CurrentTetris.Tetris.GetUpperBound(1); j++)
                        {
                            if (gameData.CurrentTetris.Tetris[i,j])
                            {
                                if ((gameData.CurrentTetris.Coordinate.X + 2 * j) == gameData.RunningRect.X)
                                {
                                    return;
                                }
                                //方块左边是否有其他方块存在
                                else if (gameData.Screen[(gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y), (gameData.CurrentTetris.Coordinate.X + j * 2 - gameData.RunningRect.X) / 2 - 1])
                                {
                                    return;
                                }
                                else
                                    break;
                            }
                        }
                    }
                    gameData.CurrentTetris.Coordinate.X -= 2;
                    break;

                case ConsoleKey.RightArrow:
                    for (int i = 0; i <= gameData.CurrentTetris.Tetris.GetUpperBound(0); i++)
                    {      //j从大到小判断，相当于X从右向左判断，可以节省判断次数
                        for (int j = gameData.CurrentTetris.Tetris.GetUpperBound(1); j >=0; j--)
                        {
                            if (gameData.CurrentTetris.Tetris[i,j])
                            { //在逻辑上坐标应为X + width - 1，但由于■占两个字符宽度，所以为了匹配比较坐标，坐标变成了X + width - 2，
                              //也可以把比较前面的坐标加1，而后面的坐标依然为X + width - 1
                                if ((gameData.CurrentTetris.Coordinate.X + 2 * j) == (gameData.RunningRect.X + gameData.RunningRect.Width - 2))
                                {
                                    return;
                                }
                                //判断方块右边是否存在方块
                                else if (gameData.Screen[(gameData.CurrentTetris.Coordinate.Y + i - gameData.RunningRect.Y), (gameData.CurrentTetris.Coordinate.X + 2 * j - gameData.RunningRect.X) / 2 + 1])
                                {
                                    return;
                                }
                                else
                                    break;
                            }
                        }
                    }
                    gameData.CurrentTetris.Coordinate.X += 2;
                    break;
                case ConsoleKey.R:timer.Stop();
                                ReStartGame();
                    return;
                case ConsoleKey.Q:keyDownThread.Abort();
                    return;
            }
            Paint(Type.OneTetris, gameData.CurrentTetris);
        }

        //重新开始游戏
        private void ReStartGame()
        {

            gameData.InitData();
            InitGameUI();
            timer.Start();

        }

        //判断游戏是否结束，第一行有方块存在为true则游戏结束
        private bool IsGameOver()
        {
            for (int j = 0; j <=gameData.Screen.GetUpperBound(1); j++)
            {
                if (gameData.Screen[0,j])
                {
                    return true;
                }
               
            }
            return false;
        }

        //绘制游戏画面，绘制类型，一个方块或者整个屏幕，参数是一个元素类，描绘一个俄罗斯方块的组成信息和相关方法
        private void UIPaint(Type type,Element element)
        {
            switch (type)
            {
                case Type.ALLWallTetris:
                    for (int i = 0; i <= gameData.Screen.GetUpperBound(0); i++)
                    {
                        for (int j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                        {
                            if (gameData.Screen[i,j])  
                            {
                                Console.SetCursorPosition(gameData.RunningRect.X + 2 * j, gameData.RunningRect.Y + i);
                                Console.WriteLine(Element.Square);
                            }
                        }
                    }
                    break;
                case Type.OneTetris:
                    for (int i = 0; i <= gameData.FormerElement.Tetris.GetUpperBound(0); i++)
                    {
                        for (int j = 0; j <= gameData.FormerElement.Tetris.GetUpperBound(1); j++)
                        {
                            if (gameData.FormerElement.Tetris[i,j])
                            {
                                Console.SetCursorPosition(gameData.FormerElement.Coordinate.X + 2 * j, gameData.FormerElement.Coordinate.Y + i);
                                Console.Write("  ");
                            }
                        }
                    }
                    for (int i = 0; i <= element.Tetris.GetUpperBound(0); i++)
                    {
                        for (int j = 0; j <= element.Tetris.GetUpperBound(1); j++)
                        {
                            if (element.Tetris[i,j])
                            {
                                Console.SetCursorPosition(element.Coordinate.X + 2 * j, element.Coordinate.Y + i);
                                Console.Write(Element.Square);
                            }
                        }
                    }
                    break;
                case Type.ClearAllWllTetris:
                    for (int i = 0; i <= gameData.FormerScreen.GetUpperBound(0); i++)
                    {
                        for (int j = 0; j <= gameData.FormerScreen.GetUpperBound(1); j++)
                        {
                            if (gameData.FormerScreen[i, j])
                            {
                                Console.SetCursorPosition(gameData.RunningRect.X + 2 * j, gameData.RunningRect.Y + i);
                                Console.WriteLine("  ");
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            Console.SetCursorPosition(gameData.ShowRect.X + 8, gameData.ShowRect.Y + gameData.ShowRect.Height - 3);
            Console.Write(gameData.Score);
        }

        //重构整个俄罗斯方块组成的墙面
        private void RestructWallEvent()
        {
            int i, j, k, count;
            bool restruct = false;
            //将现在的Screen数据拷贝到FormerScreen数组
            for (i = 0; i <= gameData.Screen.GetUpperBound(0); i++)
            {
                for (j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                {
                    gameData.FormerScreen[i, j] = gameData.Screen[i, j];
                }
            }
            for (i = gameData.Screen.GetUpperBound(0); i >= 0; i--)
            {
                count = 0;
                for (j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                {
                    if (gameData.Screen[i, j])
                    {
                        count++;
                    }
                }

                if (count == GameData.Col)
                {
                    restruct = true;
                    gameData.Score++;//游戏分数为消去一行增加一分
                    for (j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                    {
                        gameData.Screen[i, j] = false;
                    }
                }

                else if (count == 0)
                {
                    break;
                }

            }
            if (!restruct)
            {
                return;
            }
            //将要重构，先把墙面清除
            Paint(Type.ClearAllWllTetris, null);
            //下面的方法为将所有的方块向下移动（如果可以移动的话）
            for (i = GameData.Row - 1; i > 0; i--)
            {
                if (IsEmpty(i))
                {
                    for (k = i - 1; k >= 0; k--)
                    {
                        if (!IsEmpty(k))
                        {
                            break;
                        }
                    }
                    if (k < 0)
                    {
                        break;
                    }
                    for (j = 0; j <= gameData.Screen.GetUpperBound(1); j++)
                    {
                        gameData.Screen[i, j] = gameData.Screen[k, j];
                        gameData.Screen[k, j] = false;
                    }

                }
            }

                Paint(Type.ALLWallTetris, null);
        }

        //判断屏幕数组的某一行是否为空
        private bool IsEmpty(int row)
        {

            for (int j = 0; j <=gameData.Screen.GetUpperBound(1) ; j++)
            {
                if (gameData.Screen[row,j])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
