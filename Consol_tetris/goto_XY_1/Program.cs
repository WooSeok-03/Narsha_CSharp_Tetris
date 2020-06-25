using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace ConsoleApp2
{
    class Program
    {
        public static Random r = new Random();
        public static int overlap_count = 0;
        public static int line_count = 0;
        public static int rotate_num = 0;
        public static int another_block = r.Next(0,7);
        public static void make_block(int x, int y, int[,,,] block_L)
        {
            int i, j;   //i : 가로 / j : 세로
            for (j = 0; j < 4; j++)
            {
                for (i = 0; i < 4; i++)
                {
                    if (block_L[another_block ,rotate_num, j, i] == 1)
                    {
                        Console.SetCursorPosition(x + i, y + j);
                        Console.Write("*");
                    }
                }
                Console.WriteLine("");
            }
        }
        public static void delete_block(int x, int y, int[,,,] block_L)
        {
            int i, j;   //i : 가로 / j : 세로
            for (j = 0; j < 4; j++)
            {
                for (i = 0; i < 4; i++)
                {
                    if (block_L[another_block, rotate_num, j, i] == 1)
                    {
                        Console.SetCursorPosition(x + i, y + j);
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("");
            }
            //---------------------------------------------------------
            y++;
        }

        public static void random_block()
        {
            Random r = new Random();
            another_block = r.Next(0, 6);
        }
        public static int overlap_check (int x, int y, int rotate_num, int[,,,] block_L, int[,] background)
        {
            int overlap_count = 0;
            for(int j = 0; j < 4; j++)
            {
                for(int i = 0; i < 4; i++)
                {
                    if ( + y < 12)
                    {
                        if (block_L[another_block, rotate_num, j, i] == 1 && background[j + y, i + x] == 1)
                        {
                            overlap_count++;
                        }
                    }
                }
            }
            return overlap_count;
        }

        public static void insert_block(int x, int y, int[,,,]block_L, int[,] background)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (block_L[another_block, rotate_num, j, i] == 1)
                    {
                        background[j + y, x + i] = 1;
                    }
                }
            }
        }
        public static void delete_line(int[,] background)
        {
            for (int k = 1; k <= 10; k++)
            {
                line_count = 0;
                for (int i = 1; i <= 10; i++)
                {
                    if (background[k, i] == 1)   //가장 아래있는 블럭이 1인 경우
                    {
                        line_count++;
                    }
                }
                if (line_count == 10)    //한줄이 채워졌을 경우(10개의 블럭이 한줄에 있을 경우)
                {
                    for (int j = k; j > 1; j--)
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            background[j, i] = background[j - 1, i];  //가장 아랫줄을 없애고 한칸씩 모든 블럭을 내림
                        }
                    }
                    draw_background(background);    //배경을 다시 그림.
                }
            }
        }

        public static void draw_background(int[,] background)
        {
            for (int j = 0; j < 12; j++)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (background[j, i] == 1)
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write("*");
                    }
                    else
                    {
                        Console.SetCursorPosition(i, j);
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("");
            }
        }

        public static void print_background(int[,] background)
        {
            for (int j = 0; j < 12; j++)
            {
                for (int i = 0; i < 12; i++)
                {
                    if (background[j, i] == 1)
                    {
                        Console.SetCursorPosition(i, j + 13);
                        Console.Write("1");
                    }
                    else
                    {
                        Console.SetCursorPosition(i, j + 13);
                        Console.Write("0");
                    }
                }
                Console.WriteLine("");
            }
        }
        
        public static int next_block(int next, int[,,,] block_L)
        {
            next = r.Next(0, 7);

            delete_block(13, 0, block_L);
            make_block(13, 0, block_L);

            return next;
        }

        public static void print_next_block(int next, int[,,,] block_L)
        {
            int i, j;   //i : 가로 / j : 세로

            for (j = 0; j < 4; j++)
            {
                for (i = 0; i < 4; i++)
                {
                    if (block_L[another_block, rotate_num, j, i] == 1)
                    {
                        Console.SetCursorPosition(i + 13, j);
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("");
            }

            //---------------------------------------------------------

            for (j = 0; j < 4; j++)
            {
                for (i = 0; i < 4; i++)
                {
                    if (block_L[another_block, 0, j, i] == 1)
                    {
                        Console.SetCursorPosition(i + 13, j);
                        Console.Write("*");
                    }

                    else if (block_L[another_block, 0, j, i] == 0)
                    {
                        Console.SetCursorPosition(i + 13, j);
                        Console.Write(" ");
                    }
                }
                Console.WriteLine("");
            }
            //---------------------------------------------------------
        }

        static void Main(string[] args)
        {
            int count = 0;
            //x, y 는 전체 백그라운드의 좌표를 나타냄 (x가 늘어날수록 오른쪽, y가 늘어날수록 아래쪽)
            int x = 4;  //x좌표 초기값
            int y = 1;  //y좌표 초기값 

            int[,,,] block_L = new int[7, 4 ,4, 4]
            {

                //■
                //■■■
                {
                    {
                    {0,0,0,0},
                    {0,1,0,0},
                    {0,1,1,1},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,1,1,0},
                    {0,1,0,0},
                    {0,1,0,0}
                    },

                    {
                    {0,0,0,0},
                    {1,1,1,0},
                    {0,0,1,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,1,0},
                    {0,0,1,0},
                    {0,1,1,0},
                    {0,0,0,0}
                    },
                },

                //    ■
                //■■■
                {
                    {
                    {0,0,0,0},
                    {0,0,1,0},
                    {1,1,1,0},
                    {0,0,0,0}
                    },

                    {
                    {0,1,0,0},
                    {0,1,0,0},
                    {0,1,1,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,1,1,1},
                    {0,1,0,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,1,1,0},
                    {0,0,1,0},
                    {0,0,1,0}
                    },
                },

                //  ■
                //■■■
                {
                    {
                    {0,0,0,0},
                    {0,0,1,0},
                    {0,1,1,1},
                    {0,0,0,0}
                    },

                    {
                    {0,1,0,0},
                    {0,1,1,0},
                    {0,1,0,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,1,1,1},
                    {0,0,1,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,0,1,0},
                    {0,1,1,0},
                    {0,0,1,0}
                    },
                },

                //■■
                //  ■■
                {
                    {
                    {0,0,0,0},
                    {0,1,1,0},
                    {0,0,1,1},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,0,1,0},
                    {0,1,1,0},
                    {0,1,0,0}
                    },

                    {
                    {0,0,0,0},
                    {1,1,0,0},
                    {0,1,1,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,0,1,0},
                    {0,1,1,0},
                    {0,1,0,0}
                    },
                },

                //  ■■
                //■■
                {
                    {
                    {0,0,0,0},
                    {0,0,1,1},
                    {0,1,1,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,1,0,0},
                    {0,1,1,0},
                    {0,0,1,0}
                    },

                    {
                    {0,0,0,0},
                    {0,1,1,0},
                    {1,1,0,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,1,0,0},
                    {0,1,1,0},
                    {0,0,1,0}
                    },
                },

                //■■
                //■■
                {
                    {
                    {0,0,0,0},
                    {0,1,1,0},
                    {0,1,1,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,1,1,0},
                    {0,1,1,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,1,1,0},
                    {0,1,1,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,0,0},
                    {0,1,1,0},
                    {0,1,1,0},
                    {0,0,0,0}
                    },
                },

                //■■■■
                {
                    {
                    {0,0,0,0},
                    {0,0,0,0},
                    {1,1,1,1},
                    {0,0,0,0}
                    },

                    {
                    {0,1,0,0},
                    {0,1,0,0},
                    {0,1,0,0},
                    {0,1,0,0}
                    },

                    {
                    {0,0,0,0},
                    {1,1,1,1},
                    {0,0,0,0},
                    {0,0,0,0}
                    },

                    {
                    {0,0,1,0},
                    {0,0,1,0},
                    {0,0,1,0},
                    {0,0,1,0}
                    },
                }
            };

            int[,] background = new int[12, 12]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1},
                {1,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,1},
                {1,1,1,1,0,0,0,1,1,1,1,1},
                {1,1,1,1,1,1,0,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1}
            };

            ConsoleKeyInfo key_value;
            String ch;

            draw_background(background);
            
            //---------------------------------------------------------
            
            while (true)
            {
                if (count == 10)
                {
                    count = 0;

                    overlap_count = overlap_check(x, y+1, rotate_num, block_L, background);

                    if (overlap_count == 0)
                    {
                        delete_block(x, y, block_L);
                        y++;
                        make_block(x, y, block_L);
                    }
                    else
                    {
                        insert_block(x, y, block_L, background);
                        delete_line(background);
                        print_background(background);
                        another_block = r.Next(0, 7);
                        print_next_block(another_block, block_L);
                        //----------------------------------------------

                        x = 4;
                        y = 1;
                        rotate_num = 0;
                    }
                }

                if (Console.KeyAvailable)
                {

                    key_value = Console.ReadKey(true);
                    ch = key_value.Key.ToString();

                    if (ch == "A")
                    {
                        overlap_count = overlap_check(x-1, y, rotate_num, block_L, background);

                        if (overlap_count == 0)
                        {
                            delete_block(x, y, block_L);
                            x--;
                            make_block(x, y, block_L);
                        }
                    }
                    else if (ch == "D")
                    {
                        overlap_count = overlap_check(x+1, y, rotate_num, block_L, background);

                        if (overlap_count == 0)
                        {
                            delete_block(x, y, block_L);
                            x++;
                            make_block(x, y, block_L);
                        }
                    }
                    else if (ch == "W")
                    {
                        overlap_count = overlap_check(x, y - 1, rotate_num, block_L, background);

                        if (overlap_count == 0)
                        {
                            delete_block(x, y, block_L);
                            y--;
                            make_block(x, y, block_L);
                        }
                    }
                    else if (ch == "S")
                    {
                        overlap_count = overlap_check(x, y + 1, rotate_num, block_L, background);

                        if (overlap_count == 0)
                        {
                            delete_block(x, y, block_L);
                            y++;
                            make_block(x, y, block_L);
                        }
                    }
                    else if(ch == "R")
                    {
                        int temp_rotate_num = rotate_num + 1;
                        if(temp_rotate_num == 4) { temp_rotate_num = 0; }
                        if(overlap_count == 0)
                        delete_block(x, y, block_L);
                        rotate_num++;
                        if(rotate_num == 4) { rotate_num = 0; }
                        make_block(x, y, block_L);
                    }
                }

                count++;
                Thread.Sleep(100);


            }


        }
    }
}
