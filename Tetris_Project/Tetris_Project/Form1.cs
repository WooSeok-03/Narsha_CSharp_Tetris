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

        int x = 0;
        int y = 0;

        int[,] block_L = new int[4, 4]
        {
            {0,0,0,0 },
            {0,1,0,0 },
            {0,1,1,1 },
            {0,0,0,0 }
        };

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

                    Controls.Add(box);  //Control의 리스트(Controls)에 Box를 추가 (Control은 WinForm에 있는 모든 레이아웃)

                    box.BringToFront(); //앞으로 보내기(배경 앞으로)

                    boxs[j, i] = box;
                }
            }

            for(int j = 0; j < 4; j++)
            {
                for(int i = 0; i < 4; i++)
                {
                    if(block_L[j,i] == 0)
                    {
                        boxs[y + j, x + i].BackColor = Color.White; //블럭 배열의 값이 0인경우 흰색
                    }
                    else
                    {
                        boxs[y + j, x + i].BackColor = Color.Red;   //블럭 배열의 값이 1인 경우 빨간색
                    }
                }
            }


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
                        if(block_L[j, i] == 1)
                        {
                            remember_block[y + j, x + i] = 1;
                        }
                    }
                }
                x = y = 0;
            }
        }

        private bool overlap_check(int x, int y)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (block_L[j, i] == 1)
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
                    if (block_L[j, i] == 1) //기존에 있던 부분의 블럭(1)을
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
                    if (block_L[j, i] == 1) //이동후 블럭(1)인 부분만 (하얀색(0)부분 제외)
                    {
                        boxs[y +j , x + i].BackColor = Color.Red; //블럭의 배열값이 1인 부분을 빨간색으로
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
        }
    }
}
