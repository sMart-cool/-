using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{

    public struct Rect
    {
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public Rect(int x,int y,int width,int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    //游戏数据类，保存游戏相关的数据和状态
    class GameData
    {
        //获取设置当前的分数
        public int Score { get; set; }
        //当前游戏运行的画面矩形的大小及坐标
        private Rect runningRect = new Rect(20, 5, 20, 20);
        public Rect RunningRect
        {
            get { return runningRect; }
        }
        //显示下一个方块的画面矩形大小及坐标
        private Rect showRect = new Rect(41, 5, 10, 20);
        public Rect ShowRect
        {
            get { return showRect; }
        }
        //获取游戏中容纳方块元素的行数，即游戏画面大小的行数
        public static int Row = 20;
        //获取游戏中容纳方块元素的列数，即游戏画面大小的列数
        public static int Col = 10;
        //二维数组，记录整个游戏屏幕的方块元素的位置以及有无，有为true，无为fasle（大数组）
        public bool[,] Screen = new bool[Row, Col];
        //记录上一次屏幕的方块元素信息
        public bool[,] FormerScreen = new bool[Row, Col];
        //当前在屏幕上运动的一个俄罗斯方块
        public Element CurrentTetris;
        //下一次将要出现的俄罗斯方块
        public Element NextTetris;
        //记录一个Element类，里面包含一个俄罗斯方块上次的类型，坐标和4*4Tetris数组等信息
        public Element FormerElement = new Element();


        public GameData()//构造函数
        {
            Score = 0;
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    Screen[i, j] = false;
                }
            }
        }

        //初始化游戏屏幕
        public void InitData()
        {
            Score = 0;
            for (int i = 0; i < Row; i++)
            {
                for (int j = 0; j < Col; j++)
                {
                    Screen[i, j] = false;
                }
            }
        }
    }
}
