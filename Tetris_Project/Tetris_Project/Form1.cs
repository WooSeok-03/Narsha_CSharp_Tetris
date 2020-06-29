using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris_Project
{
    public partial class Form1 : Form
    {
        const int BASE_X = 12;  //윈폼에 있는 배경 Location X위치
        const int BASE_Y = 7;   //윈폼에 있는 배경 Location Y위치

        PictureBox[,] boxs = new PictureBox[20, 10];

        int[,] remember_block = new int[20, 10];    //블럭이 땅에 닿은을 기록할 변수

        Timer t = new Timer();

        Random random_block = new Random();

        int x = 0;
        int y = 0;

        int current_block = 0;  //현재 블럭 모양(인덱스)
        int next_block = 0;     //다음 블럭 모양(미리보기용)

        //■
        //■■■
        /*
        int[,] block_L = new int[4, 4]
        {
            { 0,0,0,0 },
            { 0,1,0,0 },
            { 0,1,1,1 },
            { 0,0,0,0 }
        };
        */

        int[,,] block_n = new int[7, 4, 4]
            {
                //■
                //■■■
                {
                    {0, 0, 0, 0},
                    {0, 1, 0, 0},
                    {0, 1, 1, 1},
                    {0, 0, 0, 0},
                },

                //■■
                //■■
                {
                    {0, 0, 0, 0},
                    {0, 1, 1, 0},
                    {0, 1, 1, 0},
                    {0, 0, 0, 0},
                },

                //■■■■
                {
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {1, 1, 1, 1},
                    {0, 0, 0, 0},
                },

                //■
                //■■
                //  ■
                {
                    {0, 0, 0, 0},
                    {0, 1, 0, 0},
                    {0, 1, 1, 0},
                    {0, 0, 1, 0},
                },

                //  ■
                //■■
                //■
                {
                    {0, 0, 0, 0},
                    {0, 0, 1, 0},
                    {0, 1, 1, 0},
                    {0, 1, 0, 0},
                },

                //    ■
                //■■■
                {
                    {0, 0, 0, 0},
                    {0, 0, 1, 0},
                    {1, 1, 1, 0},
                    {0, 0, 0, 0},
                },

                //  ■
                //■■■ 
                {
                    {0, 0, 0, 0},
                    {0, 0, 1, 0},
                    {0, 1, 1, 1},
                    {0, 0, 0, 0},
                }
            };

        Color[] block_color = new Color[8]
        {
            Color.White,
            Color.Red,
            Color.Orange,
            Color.Gold,
            Color.Green,
            Color.SkyBlue,
            Color.Blue,
            Color.Purple
        };

        Color[,] saved_color = new Color[20, 10];

        private void resetBlock()
        {
            block_n = new int[7, 4, 4]
            {
                {
                    {0, 0, 0, 0},
                    {0, 1, 0, 0},
                    {0, 1, 1, 1},
                    {0, 0, 0, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 1, 1, 0},
                    {0, 1, 1, 0},
                    {0, 0, 0, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 0, 0, 0},
                    {1, 1, 1, 1},
                    {0, 0, 0, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 1, 0, 0},
                    {0, 1, 1, 0},
                    {0, 0, 1, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 0, 1, 0},
                    {0, 1, 1, 0},
                    {0, 1, 0, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 0, 1, 0},
                    {1, 1, 1, 0},
                    {0, 0, 0, 0},
                },
                {
                    {0, 0, 0, 0},
                    {0, 0, 1, 0},
                    {0, 1, 1, 1},
                    {0, 0, 0, 0},
                }
            };
        }

        private void drawNextBlock()
        {
            Bitmap map = new Bitmap(90, 90);
            for(int j = 0; j < 90; j++)
            {
                for(int i = 0; i < 90; i++)
                {
                    map.SetPixel(i, j, Color.Gray); //미리보기 칸을 회색으로 채움
                }
            }

            int x = 2;
            int y = 2;

            for(int j = 0; j < 4; j++)
            {
                for(int i = 0; i < 4; i++)
                {
                    for(int h = 0; h < 20; h++)
                    {
                        for(int w = 0; w < 20; w++)
                        {
                            if(block_n[next_block, j, i] == 1)
                            {
                                map.SetPixel(x + w, y + h, block_color[next_block + 1]);
                            }
                            else
                            {
                                map.SetPixel(x + w, y + h, block_color[0]);
                            }
                        }
                    }
                    x += 22;
                }
                x = 2;
                y += 22;
            }
            pictureBox2.Image = map;    //picturBox2에 우리가 만든 Bitmap을 띄움
        }

        public Form1()
        {
            InitializeComponent();

            for (int j = 0; j < 20; j++)    //y축 블럭 갯수 만큼 반복
            {
                for (int i = 0; i < 10; i++)    //x축 블럭 갯수 만큼 반복
                {
                    PictureBox box = new PictureBox();  //배경 객체 생성
                    box.Location = new Point((BASE_X + 2) + (i * 22), (BASE_Y + 2) + (j * 22)); //(기본위치 + 2(경계선 픽셀)) + 이동할 위치(* 22(한 블럭의 픽셀크기))
                    box.Size = new Size(20, 20);   //블럭 1칸 크기

                    box.BackColor = Color.White;  //블럭 색깔을 지정
                    saved_color[j, i] = Color.White;
                    Controls.Add(box);  //Control의 리스트(Controls)에 Box를 추가 (Control은 WinForm에 있는 모든 레이아웃)

                    box.BringToFront(); //앞으로 보내기(배경 앞으로)

                    boxs[j, i] = box;
                }
            }

            current_block = random_block.Next(7);   //처음 블럭을 랜덤으로
            for(int j = 0; j < 4; j++)
            {
                for(int i = 0; i < 4; i++)
                {
                    if(block_n[current_block, j,i] == 0)
                    {
                        boxs[y + j, x + i].BackColor = Color.White; //블럭 배열의 값이 0인경우 흰색
                    }
                    else
                    {
                        boxs[y + j, x + i].BackColor = block_color[current_block + 1];   //블럭 배열의 값이 1인 경우 빨간색
                    }
                }
            }

            next_block = random_block.Next(7);  //다음 블럭을 랜덤으로
            drawNextBlock();

            //------ Timer -------
            t.Interval = 1000;  // Interval : 몇 ms 마다 호출 될지 설정
            t.Tick += new EventHandler(timer_event);    //타이머가 호출될 때 마다 실행할 이벤트 추가
            t.Start();  //타이머 실행
        }

        private void timer_event(object sender, EventArgs args) //매게변수는 타이머에서 자동으로 보내줌
        {
            //move_block(0, 1); ;    //1초마다 y를 한칸씩 내림
            if (!move_block(0, 1))  //블럭이 바닥에 닿았을 경우
            {
                for(int j = 0; j < 4; j++)
                {
                    for(int i = 0; i < 4; i++)
                    {
                        if(block_n[current_block, j, i] == 1)
                        {
                            remember_block[y + j, x + i] = 1;
                            saved_color[y + j, x + i] = block_color[current_block + 1];
                        }
                    }
                }

                //------- Block 설치 이후 ----------
                //맨 아래쪽 한 줄만 체크
                /*
                int block_count = 0;
                for(int i = 0; i < 10; i++)
                {
                    if (remember_block[19, i] == 1) block_count++;
                }

                if(block_count == 10)
                {
                    for (int j = 19; j > 0; j--)    //아래쪽부터 내리기 위해 Y를 19부터 1까지 반복
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            remember_block[j, i] = remember_block[j - 1, i];  //모든 블럭을 한칸씩 내림
                        }
                    }
                }
                */
                
                //모든 줄 체크
                for(int j = 0; j < 20; j++) //모든 줄을 탐색
                {
                    int block_count = 0;
                    for(int i = 0; i < 10; i++)
                    {
                        if (remember_block[j, i] == 1) block_count++;
                    }

                    if(block_count == 10)
                    {
                        for(int level = j; level > 0; level--)  //라인이 없어졌을때, 없어진 라인부터 한칸씩 내려야 하기때문 
                        {
                            for(int i = 0; i < 10; i++)
                            {
                                remember_block[level, i] = remember_block[level - 1, i];    //모든 블럭을 한칸씩 내림
                                saved_color[level, i] = saved_color[level - 1, i];
                            }
                        }
                    }
                }

                redraw_background();

                //current_block = random_block.Next(7);   // Next(n) : 0부터 n - 1 까지의 랜덤의 수를 반환

                current_block = next_block;
                next_block = random_block.Next(7);
                drawNextBlock();

                x = y = 0;
                resetBlock();
            }
        }

        private void redraw_background()
        {
            for(int j = 0; j < 20; j++)
            {
                for(int i = 0; i < 10; i++)
                {
                    boxs[j, i].BackColor = saved_color[j, i];
                }
            }
        }

        private int[,] rotate_block()
        {
            int[,] arr = new int[4, 4];
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    //회전 전의 열 번호와 회전후의 행 번호가 일치
                    //회전 전의 X 번호와 회전후의 Y번호가 일치
                    //열 : i, 행 : j
                    //회전 후의 열 번호가 N - 1 에서 회전 전의 행을 뺀 값
                    arr[j, i] = block_n[current_block,3  - i, j]; 
                }
            }
            return arr;
        }

        //메서드 Overloading
        private bool overlap_check(int x, int y)
        {
            return overlap_check(x, y, false);
        }

        private bool overlap_check(int x, int y, bool is_rotate)
        {
            int[,] arr = new int[4,4];  
            //arr = block_L;  //기본적인 블럭을 arr에 넣음

            //arr에 3차원 배열인 block_n을 수동으로 대입해줌
            for(int j = 0; j < 4; j++)
            {
                for(int i = 0; i < 4; i++)
                {
                    arr[j, i] = block_n[current_block, j, i];
                }
            }

            if (is_rotate) arr = rotate_block();    //만약 회전했다면 arr에 회전한 블럭을 넣음

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (arr[j, i] == 1) //회전한 블럭으로 overlap_check
                    {
                        if (x + i >= 10 || x + i < 0) return true;  //X 부분에서 맵을 이탈
                        if (y + j >= 20 || y + i < 0) return true;  //Y 부분에서 맵을 이탈
                        if (remember_block[y + j, x + i] == 1) return true; //쌓여졌던 블럭과 닿았으 때
                    }
                }
            }

            return false;
        }

        private bool move_block(int x_amount, int y_amount)
        {
            if (overlap_check(x + x_amount, y + y_amount)) return false; //이동할 좌표의 값을 넘겨줌
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (block_n[current_block, j, i] == 1) //기존에 있던 부분의 블럭(1)을
                    {
                        boxs[y + j, x + i].BackColor = Color.White; //기존에 있던 블럭을 White로 변경
                    }
                }
            }
            x += x_amount;
            y += y_amount;
            for (int j = 0; j < 4; j++)
            {
                for(int i = 0; i < 4; i++)
                {
                    if (block_n[current_block, j, i] == 1) //이동후 블럭(1)인 부분만 (하얀색(0)부분 제외)
                    {
                        boxs[y +j , x + i].BackColor = block_color[current_block+1]; // 현재 블럭의 색깔로 바꿈.
                    }
                }
            }

            return true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)  // 키보드 W 가 눌렸을 때
            {
                move_block(0, -1);  //y축 위로 한칸 이동
            }
            else if (e.KeyCode == Keys.A)    // 키보드 A 가 눌렸을 때
            {
                move_block(-1, 0);  //x축 왼쪽으로 한칸 이동
            }
            else if (e.KeyCode == Keys.S)   // 키보드 S 가 눌렸을 때
            {
                move_block(0, 1);   //y축 아래로 한칸 이동
            }
            else if (e.KeyCode == Keys.D)   // 키보드 D 가 눌렸을 때
            {
                move_block(1, 0);   //x축 오른쪽으로 한칸 이동
            }
            else if(e.KeyCode == Keys.R)    // 키보드 R 을 눌렀을 때
            {
                if (!overlap_check(x, y, true))
                {
                    for (int j = 0; j < 4; j++)   //기존에 있던 블럭 삭제
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (block_n[current_block, j, i] == 1)
                            {
                                boxs[y + j, x + i].BackColor = Color.White;
                            }
                        }
                    }

                    int[,] arr = rotate_block();    //모든 블럭 회전
                    //block_L = rotate_block();   // L 블럭 회전

                    //수동으로 초기화
                    for(int j = 0; j < 4; j++)
                    {
                        for(int i = 0; i < 4; i++)
                        {
                            block_n[current_block, j, i] = arr[j, i];
                        }
                    }

                    for (int j = 0; j < 4; j++)     //회전후 블럭 생성
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            if (block_n[current_block, j, i] == 1)
                            {
                                boxs[y + j, x + i].BackColor = block_color[current_block+1];
                            }
                        }
                    }
                }
            }
        }
    }
}
