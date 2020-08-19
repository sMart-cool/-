using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{

    public struct Point
    {
        public int X;
        public int Y;
        public Point(int x,int y)
        {
            X = x;
            Y = y;
        }
    }
    /// <summary>
    /// 元素类，描绘一个俄罗斯方块的组成信息和相关方法
    /// </summary>
     public  class Element
    {
        //构成俄罗斯方块的基本属性
        public static char Square = '■';
        //4*4数组，描述了一个俄罗斯方块的构成
        public bool[,] Tetris = new bool[4, 4];
        //4*4数组的坐标，借此得到一个俄罗斯方块的坐标
        public Point Coordinate;
        //描述一个俄罗斯方块的类型，有七类，分别编号1-7
        public int Type;
        //俄罗斯方块类型如下：
        //■■   ■                ■ ■       ■■   ■
        //■■ ■■■ ■■■■ ■■■ ■■■ ■■   ■■
        // 1     2       3       4      5      6    ■ 7
        //              
        //形成一个俄罗斯方块
        //x:方块的起始横坐标，Y：方块的起始纵坐标

        public void GenerateTetris(int x,int y)
        {
            //int i, j;
            for (int i = 0; i <= this.Tetris.GetUpperBound(0); i++)//获取二维数组第一维最大下标
            {
                for (int j = 0; j <= this.Tetris.GetUpperBound(1); j++)
                {
                    this.Tetris[i, j] = false;
                }
            }
            Random random = new Random();
            Type = random.Next(1, 8);//返回一个一到七的随机类型
            switch (Type)
            {
                case 1:
                    {
                        this.Tetris[0, 0] = true;
                        this.Tetris[0, 1] = true;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 1] = true;
                    }
                    break;
                case 2:
                    {
                        this.Tetris[0, 1] = true;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 1] = true;
                        this.Tetris[1, 2] = true;
                    }
                    break;
                case 3:
                    {
                        this.Tetris[0, 0] = true;
                        this.Tetris[0, 1] = true;
                        this.Tetris[0, 2] = true;
                        this.Tetris[0, 3] = true;
                    }
                    break;
                case 4:
                    {
                        this.Tetris[0, 2] = true;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 1] = true;
                        this.Tetris[1, 2] = true;
                    }
                    break;
                case 5:
                    {
                        this.Tetris[0, 0] = true;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 1] = true;
                        this.Tetris[1, 2] = true;
                    }
                    break;
                case 6:
                    {
                        this.Tetris[0, 1] = true;
                        this.Tetris[0, 2] = true;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 1] = true;
                    }
                    break;
                case 7:
                    {
                        this.Tetris[0, 1] = true;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 1] = true;
                        this.Tetris[2, 0] = true;
                    }
                    break;
                default:
                    break;
            }
            this.Coordinate = new Point(x, y);
        }

        //翻转一个俄罗斯方块
        public void TransformTetris()
        {
            switch (Type)
            {
                //从二开始，因为类型1不需要旋转
                case 2:
                    if (!this.Tetris[2,1])
                    {
                        this.Tetris[2, 1] = true;
                        this.Tetris[1, 0] = false;
                    }
                    else if (!this.Tetris[1, 2])
                    {
                        this.Tetris[1, 2] = true;
                        this.Tetris[2, 1] = false;
                    }
                    else if (!this.Tetris[0,1])
                    {
                        this.Tetris[0, 1] = true;
                        this.Tetris[1, 2] = false;
                    }
                    else
                    {
                        this.Tetris[1, 0] = true;
                        this.Tetris[0, 1] = false;
                    }
                    break;
                case 3:
                    if (!this.Tetris[0,0])
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            this.Tetris[i, 1] = false;
                        }
                        for (int j = 0; j < 4; j++)
                        {
                            this.Tetris[0, j] = true;
                        }
                    }
                    else
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            this.Tetris[0, j] = false;
                        }
                        for (int i = 0; i < 4; i++)
                        {
                            this.Tetris[i, 1] = true;
                        }
                    }
                    break;
                case 4:
                    if (this.Tetris[0,2])
                    {
                        this.Tetris[1, 0] = false;
                        this.Tetris[1, 2] = false;
                        this.Tetris[0, 2] = false;
                        this.Tetris[0, 0] = true;
                        this.Tetris[0, 1] = true;
                        this.Tetris[2, 1] = true;
                    }
                    else if (this.Tetris[0, 0])
                    {
                        this.Tetris[0, 0] = false;
                        this.Tetris[0, 1] = false;
                        this.Tetris[2, 1] = false;
                        this.Tetris[2, 0] = true;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 2] = true;
                    }
                    else if (this.Tetris[2, 0])
                    {
                        this.Tetris[2, 0] = false;
                        this.Tetris[1, 0] = false;
                        this.Tetris[1, 2] = false;
                        this.Tetris[0, 1] = true;
                        this.Tetris[2, 1] = true;
                        this.Tetris[2, 2] = true;
                    }
                    else
                    {
                        this.Tetris[0, 1] = false;
                        this.Tetris[2, 1] = false;
                        this.Tetris[2, 2] = false;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 2] = true;
                        this.Tetris[0, 2] = true;
                    }
                    break;
                case 5:
                    if (this.Tetris[0, 0])
                    {
                        this.Tetris[0, 0] = false;
                        this.Tetris[1, 0] = false;
                        this.Tetris[1, 2] = false;
                        this.Tetris[2, 0] = true;
                        this.Tetris[2, 1] = true;
                        this.Tetris[0, 1] = true;
                    }
                    else if (this.Tetris[2, 0])
                    {
                        this.Tetris[2, 0] = false;
                        this.Tetris[2, 1] = false;
                        this.Tetris[0, 1] = false;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 2] = true;
                        this.Tetris[2, 2] = true;
                    }
                    else if (this.Tetris[2, 2])
                    {
                        this.Tetris[1, 0] = false;
                        this.Tetris[1, 2] = false;
                        this.Tetris[2, 2] = false;
                        this.Tetris[2, 1] = true;
                        this.Tetris[0, 1] = true;
                        this.Tetris[0, 2] = true;
                    }
                    else
                    {
                        this.Tetris[0, 2] = false;
                        this.Tetris[0, 1] = false;
                        this.Tetris[2, 1] = false;
                        this.Tetris[0, 0] = true;
                        this.Tetris[1, 0] = true;
                        this.Tetris[1, 2] = true;
                    }
                    break;
                case 6:
                    if (this.Tetris[0, 2])
                    {
                        this.Tetris[0, 2] = false;
                        this.Tetris[1, 0] = false;
                        this.Tetris[0, 0] = true;
                        this.Tetris[1, 2] = true;
                    }
                    else
                    {
                        this.Tetris[0, 0] = false;
                        this.Tetris[1, 2] = false;
                        this.Tetris[0, 2] = true;
                        this.Tetris[1, 0] = true;
                    }
                    break;
                case 7:
                    if (this.Tetris[0, 1])
                    {
                        this.Tetris[0, 1] = false;
                        this.Tetris[2, 0] = false;
                        this.Tetris[0, 0] = true;
                        this.Tetris[2, 1] = true;
                    }
                    else
                    {
                        this.Tetris[0, 0] = false;
                        this.Tetris[2, 1] = false;
                        this.Tetris[0, 1] = true;
                        this.Tetris[2, 0] = true;
                    }
                    break;
                default:
                    break;
            }
        }
        //将当前Element类拷贝到目标Element类,用于将当前的俄罗斯方块拷贝到下一秒执行的俄罗斯方块
        public void CopyTo(Element element)
        {
            for (int i = 0; i <= this.Tetris.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= this.Tetris.GetUpperBound(1); j++)
                {
                    element.Tetris[i, j] = this.Tetris[i, j];
                }
            }

            element.Coordinate = this.Coordinate;
            element.Type = this.Type;
        }
    }
}
