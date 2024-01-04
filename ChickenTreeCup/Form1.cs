using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LingnanCup
{
    public partial class Form1 : System.Windows.Forms.Form
    {
        public string player = "";
        public string school = "";
        public string unit = "";

        public int temporaryRecruitmentSix = 0;
        public int temporaryRecruitmentFive = 0;
        public int temporaryRecruitmentFour = 0;//这三个是临时招募各自的数量

        public int duck = 0;
        public int bear = 0;
        public int dog = 0;//这三个是隐藏怪的数量

        public int end = 0;
        /*结局，使用位运算判断
        *1-巍峨银淞、2-深寒造像
        *4-萨米之觞、8-虚无之偶
        *16-园丁、32-哨兵、64-哨兵仅击杀
        *128-时间之沙、256迈入永恒
        *512-无垠赠礼
         */

        public int special = 0;
        /*
         * 特殊事件，使用位运算判断
         * 1-大地醒转、2-呼吸、4-夺树者
         * 8-目空一些、16-图像损毁
         * 32-天途半道无漏、64-豪华车队击杀熊
         * 128-英雄无名无漏、256-英雄无名仅通关
         * 512-正义使者无漏、1024正义使者仅通关
         * 2048-亘古仇敌
         * 4096-白棠
         */
        public int heroKill = 0;//英雄无名仅通关击杀数
        public int justiceKill = 0;//正义使者仅通关击杀数

        public double[] specialBonus = { 40, 50, 60, 3.02, 3.02, 40, 50, 150, 30, 250, 90, 30.2 , 250};
        private void Refresh_Special()
        {
            if((special & 256) == 0)
            {
                //仅有英雄无名仅通关开启时，该项才有效
                heroKill = 0;
            }
            label81.Text = heroKill.ToString();

            if ((special & 1024) == 0)
            {
                //仅有正义使者仅通关开启时，该项才有效
                justiceKill = 0;
            }
            label84.Text = justiceKill.ToString();

            double bonus = 0;
            for(int i = 0; i < 13; i++)
            {
                if((special & (1 << i)) != 0)
                {
                    bonus += specialBonus[i];
                    if(i == 8)
                    {
                        bonus += heroKill * 15;
                    }
                    if(i == 10)
                    {
                        bonus += 40 * justiceKill;
                    }
                }
            }
            label80.Text = bonus.ToString();
            Refresh_All_Bonus();
        }

        //每一个,低16位为分数，高16位为个数
        public int[] emergencyFight = { 40, 60, 40, 40, 60, 100, 120, 120, 140, 160, 180, 90, 50, 70, 70, 120, 120, 140, 70, 150 };
        private void emergencyFightAdd(int id)
        {
            emergencyFight[id] += (1 << 16);
            emergencyFightRefresh();
        }

        private void emergencyFightSub(int id)
        {
            if (getNum(id) != 0)
            {
                emergencyFight[id] -= (1 << 16);
            }
            emergencyFightRefresh();
        }

        private int getNum(int id)
        {
            return emergencyFight[id] >> 16;
        }

        private void emergencyFightRefresh()
        {
            int bonus = 0;
            for(int i = 0; i < emergencyFight.Length; i++)
            {
                bonus += (getNum(i) * (emergencyFight[i] & 65535));
            }
            label65.Text = bonus.ToString();
            Refresh_All_Bonus();
        }

        public int startMoney = 0;
        public int endMoney = 0;

        public void Money_Change()
        {
            if(startMoney > 999)
            {
                startMoney = 999;
            }else if(startMoney < 0)
            {
                startMoney = 0;
            }
            textBox6.Text = startMoney.ToString();

            if (endMoney > 999)
            {
                endMoney = 999;
            }
            else if (endMoney < 0)
            {
                endMoney = 0;
            }
            textBox7.Text = endMoney.ToString();


            int subBonus = 0;
            if(startMoney - endMoney >= 90)
            {
                subBonus = -(startMoney - endMoney - 90) * 20;
            }
            label88.Text = subBonus.ToString();
            Refresh_All_Bonus();
        }

        public int collection = 0;
        public int board = 0;
        public int endBonus = 0;

        private void Refresh_Collection()
        {
            if(collection < 0)
            {
                collection = 0;
            }
            textBox5.Text = collection.ToString();
            Refresh_All_Bonus();
        }

        private void Refresh_Board()
        {
            if (board < 0)
            {
                board = 0;
            }
            textBox8.Text = board.ToString();
            Refresh_All_Bonus();
        }
        private void Refresh_endBonus()
        {
            if (endBonus < 0)
            {
                endBonus = 0;
            }
            textBox4.Text = endBonus.ToString();
            Refresh_All_Bonus();
        }

        private void Refresh_All_Bonus()
        {
            if(textBox4.Text == "")
            {
                textBox4.Text = "0";
            }
            if (textBox5.Text == "")
            {
                textBox5.Text = "0";
            }
            if (textBox6.Text == "")
            {
                textBox6.Text = "0";
            }
            if (textBox7.Text == "")
            {
                textBox7.Text = "0";
            }
            if (textBox8.Text == "")
            {
                textBox8.Text = "0";
            }
            double temp = Double.Parse(label62.Text) + Double.Parse(label63.Text) + Double.Parse(label64.Text) + Double.Parse(label88.Text)
                + Double.Parse(label65.Text) + Double.Parse(label80.Text) + 10*Double.Parse(textBox5.Text) + 5*Double.Parse(textBox8.Text)
                + Double.Parse(textBox4.Text);
            label93.Text = temp.ToString();
        }


        private void Refresh_TemporaryRecruitment()
        {
            label8.Text = temporaryRecruitmentSix.ToString();
            label25.Text = temporaryRecruitmentFive.ToString();
            label27.Text = temporaryRecruitmentFour.ToString();
            int temporaryRecruitmentBonus = 10 * temporaryRecruitmentFour + 20 * temporaryRecruitmentFive + 50 * temporaryRecruitmentSix;
            label62.Text = temporaryRecruitmentBonus.ToString();
            Refresh_All_Bonus();
        }

        private void Refresh_DuckBearDog()
        {
            label10.Text = dog.ToString();
            label14.Text = duck.ToString();
            label12.Text = bear.ToString();
            int bonus = (duck + bear + dog)*20;
            label64.Text = bonus.ToString();
            Refresh_All_Bonus();
        }

        public int[] bossBonus = { 80, 200, 120, 250, 150, 650, 200, 200, 350, 80 };
        private void Refresh_End()
        {
            int endBonus = 0;
            for(int i = 0; i < 10; i++)
            {
                if((end & (1 << i)) != 0)
                {
                    endBonus += bossBonus[i];
                }
            }
            
            label63.Text = endBonus.ToString();
            Refresh_All_Bonus();
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            temporaryRecruitmentSix++;
            Refresh_TemporaryRecruitment();
        }

        private void button29_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if((end & 1) != 0)
            {
                //重复点击则取消
                button1.BackColor = Color.FromArgb(224, 224, 224);
                end &= (~1);
                Refresh_End();
                return;
            }

            if((end & 14) != 0)
            {
                end &= (~14);
                //234不能和1同时存在，因此要先清空234位
                button2.BackColor = Color.FromArgb(224, 224, 224);
                button33.BackColor = Color.FromArgb(224, 224, 224);
                button34.BackColor = Color.FromArgb(224, 224, 224);
            }
            end |= 1;
            button1.BackColor = Color.Pink;
            Refresh_End();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((end & 4) != 0)
            {
                //重复点击则取消
                button2.BackColor = Color.FromArgb(224, 224, 224);
                end &= (~4);
                Refresh_End();
                return;
            }

            if ((end & 11) != 0)
            {
                end &= (~11);
                //234不能和1同时存在，因此要先清空234位
                button1.BackColor = Color.FromArgb(224, 224, 224);
                button33.BackColor = Color.FromArgb(224, 224, 224);
                button34.BackColor = Color.FromArgb(224, 224, 224);
            }
            end |= 4;
            button2.BackColor = Color.Pink;
            Refresh_End();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if ((end & 16) != 0)
            {
                //重复点击则取消
                button3.BackColor = Color.FromArgb(224, 224, 224);
                end &= (~16);
                Refresh_End();
                return;
            }

            //3,35,68
            if ((end & 96) != 0)
            {
                end &= (~96);
                //234不能和1同时存在，因此要先清空234位
                button35.BackColor = Color.FromArgb(224, 224, 224);
                button68.BackColor = Color.FromArgb(224, 224, 224);
            }
            end |= 16;
            button3.BackColor = Color.Pink;
            Refresh_End();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if ((end & 128) != 0)
            {
                //重复点击则取消
                button4.BackColor = Color.FromArgb(224, 224, 224);
                end &= (~128);
                Refresh_End();
                return;
            }
            //时间之沙、迈入永恒与哨兵仅击杀互相不能共存
            if ((end & 320) != 0)
            {
                end &= (~320);
                button68.BackColor = Color.FromArgb(224, 224, 224);
                button36.BackColor = Color.FromArgb(224, 224, 224);
            }
            end |= 128;
            button4.BackColor = Color.Pink;
            Refresh_End();
        }


        private void button7_Click(object sender, EventArgs e)
        {
            temporaryRecruitmentSix--;
            if(temporaryRecruitmentSix < 0)
            {
                temporaryRecruitmentSix = 0;
            }
            Refresh_TemporaryRecruitment();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            temporaryRecruitmentSix = 0;
            temporaryRecruitmentFive = 0;
            temporaryRecruitmentFour = 0;
            Refresh_TemporaryRecruitment();

            end = 0;
            button1.BackColor = Color.FromArgb(224, 224, 224);
            button2.BackColor = Color.FromArgb(224, 224, 224);
            button3.BackColor = Color.FromArgb(224, 224, 224);
            button4.BackColor = Color.FromArgb(224, 224, 224);
            button33.BackColor = Color.FromArgb(224, 224, 224);
            button34.BackColor = Color.FromArgb(224, 224, 224);
            button35.BackColor = Color.FromArgb(224, 224, 224);
            button36.BackColor = Color.FromArgb(224, 224, 224);
            button68.BackColor = Color.FromArgb(224, 224, 224);
            Refresh_End();

            duck = 0;
            bear = 0;
            dog = 0;
            Refresh_DuckBearDog();


            for(int i = 0; i < emergencyFight.Length; i++)
            {
                emergencyFight[i] &= 65535;
            }
            label35.Text = "0";
            label33.Text = "0";
            label40.Text = "0";
            label38.Text = "0";
            label15.Text = "0";
            label53.Text = "0";
            label51.Text = "0";
            label49.Text = "0";
            label46.Text = "0";
            label44.Text = "0";
            label22.Text = "0";
            label59.Text = "0";
            label57.Text = "0";
            label55.Text = "0";
            label77.Text = "0";
            label75.Text = "0";
            label73.Text = "0";
            label70.Text = "0";
            label68.Text = "0";
            label66.Text = "0";
            emergencyFightRefresh();

            special = 0;
            button5.BackColor = Color.FromArgb(224, 224, 224);
            button37.BackColor = Color.FromArgb(224, 224, 224);
            button38.BackColor = Color.FromArgb(224, 224, 224);
            button44.BackColor = Color.FromArgb(224, 224, 224);
            button80.BackColor = Color.FromArgb(224, 224, 224);
            button79.BackColor = Color.FromArgb(224, 224, 224);
            button71.BackColor = Color.FromArgb(224, 224, 224);
            button70.BackColor = Color.FromArgb(224, 224, 224);
            button43.BackColor = Color.FromArgb(224, 224, 224);
            button73.BackColor = Color.FromArgb(224, 224, 224);
            button77.BackColor = Color.FromArgb(224, 224, 224);
            button74.BackColor = Color.FromArgb(224, 224, 224);
            button78.BackColor = Color.FromArgb(224, 224, 224);
            Refresh_Special();

            startMoney = 0;
            endMoney = 0;
            textBox6.Text = "0";
            textBox7.Text = "0";
            Money_Change();

            collection = 0;
            textBox5.Text = "0";
            Refresh_Collection();

            board = 0;
            textBox8.Text = "0";
            Refresh_Board();

            endBonus = 0;
            textBox4.Text = "0";
            Refresh_endBonus();

            Refresh_All_Bonus();


        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            player = textBox1.Text;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            school = textBox3.Text;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button33_Click(object sender, EventArgs e)
        {
            if ((end & 2) != 0)
            {
                //重复点击则取消
                button33.BackColor = Color.FromArgb(224, 224, 224);
                end &= (~2);
                Refresh_End();
                return;
            }

            if ((end & 13) != 0)
            {
                end &= (~13);
                button1.BackColor = Color.FromArgb(224,224,224);
                button2.BackColor = Color.FromArgb(224, 224, 224);
                button34.BackColor = Color.FromArgb(224, 224, 224);
            }
            end |= 2;
            button33.BackColor = Color.Pink;
            Refresh_End();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            if ((end & 32) != 0)
            {
                //重复点击则取消
                button35.BackColor = Color.FromArgb(224, 224, 224);
                end &= (~32);
                Refresh_End();
                return;
            }

            //3,35,68
            if ((end & 80) != 0)
            {
                end &= (~80);
                //234不能和1同时存在，因此要先清空234位
                button3.BackColor = Color.FromArgb(224, 224, 224);
                button68.BackColor = Color.FromArgb(224, 224, 224);
            }
            end |= 32;
            button35.BackColor = Color.Pink;
            Refresh_End();
        }

        private void button36_Click(object sender, EventArgs e)
        {
            if ((end & 256) != 0)
            {
                //重复点击则取消
                button36.BackColor = Color.FromArgb(224, 224, 224);
                end &= (~256);
                Refresh_End();
                return;
            }
            //时间之沙、迈入永恒与哨兵仅击杀互相不能共存
            if ((end & 192) != 0)
            {
                end &= (~192);
                button4.BackColor = Color.FromArgb(224, 224, 224);
                button68.BackColor = Color.FromArgb(224, 224, 224);
            }
            end |= 256;
            button36.BackColor = Color.Pink;
            Refresh_End();
        }

        private void label8_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            //5/37/38
            if ((special & 1) != 0)
            {
                //重复点击则取消
                button5.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~1);
                Refresh_Special();
                return;
            }

            if ((special & 6) != 0)
            {
                special &= (~6);
                //23不能和1同时存在，因此要先清空23位
                button37.BackColor = Color.FromArgb(224, 224, 224);
                button38.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 1;
            button5.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void button68_Click(object sender, EventArgs e)
        {
            if ((end & 64) != 0)
            {
                //重复点击则取消
                button68.BackColor = Color.FromArgb(224, 224, 224);
                end &= (~64);
                Refresh_End();
                return;
            }
            //哨兵仅击杀会导致4结局也无法生效，因此需要额外清空
            //3,35,68

            if ((end & 432) != 0)
            {
                end &= (~432);
                button3.BackColor = Color.FromArgb(224, 224, 224);
                button35.BackColor = Color.FromArgb(224, 224, 224);
                button4.BackColor = Color.FromArgb(224, 224, 224);
                button36.BackColor = Color.FromArgb(224, 224, 224);
            }
            end |= 64;
            button68.BackColor = Color.Pink;
            Refresh_End();
        }

        private void button71_Click(object sender, EventArgs e)
        {
            if ((special & 32) != 0)
            {
                //重复点击则取消
                button71.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~32);
                Refresh_Special();
                return;
            }
            special |= 32;
            button71.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void label92_Click(object sender, EventArgs e)
        {

        }

        private void label93_Click(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void label62_Click(object sender, EventArgs e)
        {

        }

        private void button40_Click(object sender, EventArgs e)
        {
            temporaryRecruitmentFive++;
            Refresh_TemporaryRecruitment();
        }

        private void button39_Click(object sender, EventArgs e)
        {
            temporaryRecruitmentFive--;
            if(temporaryRecruitmentFive < 0)
            {
                temporaryRecruitmentFive = 0;
            }
            Refresh_TemporaryRecruitment();
        }

        private void button42_Click(object sender, EventArgs e)
        {
            temporaryRecruitmentFour++;
            Refresh_TemporaryRecruitment();
        }

        private void button41_Click(object sender, EventArgs e)
        {
            temporaryRecruitmentFour--;
            if(temporaryRecruitmentFour < 0)
            {
                temporaryRecruitmentFour = 0;
            }
            Refresh_TemporaryRecruitment();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            bool success = int.TryParse(textBox5.Text, out collection);
            if (success)
            {
                Refresh_Collection();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            bool success = int.TryParse(textBox4.Text, out endBonus);
            if (success)
            {
                Refresh_endBonus();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            unit = textBox2.Text;
        }

        private void button13_Click_1(object sender, EventArgs e)
        {
            duck++;
            Refresh_DuckBearDog();
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            duck--;
            if(duck < 0)
            {
                duck = 0;
            }
            Refresh_DuckBearDog();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            bear++;
            Refresh_DuckBearDog();
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            bear--;
            if(bear < 0)
            {
                bear = 0;
            }
            Refresh_DuckBearDog(); 
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            dog++;
            Refresh_DuckBearDog();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            dog--;
            if(dog < 0)
            {
                dog = 0;
            }
            Refresh_DuckBearDog();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label64_Click(object sender, EventArgs e)
        {

        }

        private void button34_Click(object sender, EventArgs e)
        {
            if ((end & 8) != 0)
            {
                //重复点击则取消
                button34.BackColor = Color.FromArgb(224, 224, 224);
                end &= (~8);
                Refresh_End();
                return;
            }

            if ((end & 7) != 0)
            {
                end &= (~7);
                //234不能和1同时存在，因此要先清空234位
                button1.BackColor = Color.FromArgb(224, 224, 224);
                button2.BackColor = Color.FromArgb(224, 224, 224);
                button33.BackColor = Color.FromArgb(224, 224, 224);
            }
            end |= 8;
            button34.BackColor = Color.Pink;
            Refresh_End();
        }

        private void label63_Click(object sender, EventArgs e)
        {

        }

        private void label65_Click(object sender, EventArgs e)
        {

        }

        private void button48_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(0);
            label35.Text = getNum(0).ToString();
        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void button47_Click(object sender, EventArgs e)
        {
            emergencyFightSub(0);
            label35.Text = getNum(0).ToString();
        }

        private void button46_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(1);
            label33.Text = getNum(1).ToString();
        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void button45_Click(object sender, EventArgs e)
        {
            emergencyFightSub(1);
            label33.Text = getNum(1).ToString();
        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void button52_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(2);
            label40.Text = getNum(2).ToString();
        }

        private void button51_Click(object sender, EventArgs e)
        {
            emergencyFightSub(2);
            label40.Text = getNum(2).ToString();
        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void button50_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(3);
            label38.Text = getNum(3).ToString();
        }

        private void button49_Click(object sender, EventArgs e)
        {
            emergencyFightSub(3);
            label38.Text = getNum(3).ToString();
        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click_1(object sender, EventArgs e)
        {
            emergencyFightAdd(4);
            label15.Text = getNum(4).ToString();
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            emergencyFightSub(4);
            label15.Text = getNum(4).ToString();
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void button27_Click_1(object sender, EventArgs e)
        {
            emergencyFightAdd(5);
            label53.Text = getNum(5).ToString();
        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            emergencyFightSub(5);
            label53.Text = getNum(5).ToString();
        }

        private void label53_Click(object sender, EventArgs e)
        {

        }

        private void button25_Click_1(object sender, EventArgs e)
        {
            emergencyFightAdd(6);
            label51.Text = getNum(6).ToString();
        }

        private void button24_Click_1(object sender, EventArgs e)
        {
            emergencyFightSub(6);
            label51.Text = getNum(6).ToString();
        }

        private void button23_Click_1(object sender, EventArgs e)
        {
            emergencyFightAdd(7);
            label49.Text = getNum(7).ToString();
        }

        private void button22_Click_1(object sender, EventArgs e)
        {
            emergencyFightSub(7);
            label49.Text = getNum(7).ToString();
        }

        private void button21_Click_1(object sender, EventArgs e)
        {
            emergencyFightAdd(8);
            label46.Text = getNum(8).ToString();
        }

        private void button20_Click_1(object sender, EventArgs e)
        {
            emergencyFightSub(8);
            label46.Text = getNum(8).ToString();
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            emergencyFightAdd(9);
            label44.Text = getNum(9).ToString();
        }

        private void button18_Click_1(object sender, EventArgs e)
        {
            emergencyFightSub(9);
            label44.Text = getNum(9).ToString();
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            emergencyFightAdd(10);
            label22.Text = getNum(10).ToString();
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            emergencyFightSub(10);
            label22.Text = getNum(10).ToString();
        }

        private void button55_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(11);
            label59.Text = getNum(11).ToString();
        }

        private void button54_Click(object sender, EventArgs e)
        {
            emergencyFightSub(11);
            label59.Text = getNum(11).ToString();
        }

        private void button53_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(12);
            label57.Text = getNum(12).ToString();
        }

        private void button30_Click_1(object sender, EventArgs e)
        {
            emergencyFightSub(12);
            label57.Text = getNum(12).ToString();
        }

        private void button29_Click_2(object sender, EventArgs e)
        {
            emergencyFightAdd(13);
            label55.Text = getNum(13).ToString();
        }

        private void button28_Click_1(object sender, EventArgs e)
        {
            emergencyFightSub(13);
            label55.Text = getNum(13).ToString();
        }

        private void button67_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(14);
            label77.Text = getNum(14).ToString();
        }

        private void button66_Click(object sender, EventArgs e)
        {
            emergencyFightSub(14);
            label77.Text = getNum(14).ToString();
        }

        private void button65_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(15);
            label75.Text = getNum(15).ToString();
        }

        private void button64_Click(object sender, EventArgs e)
        {
            emergencyFightSub(15);
            label75.Text = getNum(15).ToString();
        }

        private void label75_Click(object sender, EventArgs e)
        {
            
        }

        private void button63_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(16);
            label73.Text = getNum(16).ToString();
        }

        private void button62_Click(object sender, EventArgs e)
        {
            emergencyFightSub(16);
            label73.Text = getNum(16).ToString();
        }

        private void button61_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(17);
            label70.Text = getNum(17).ToString();
        }

        private void button60_Click(object sender, EventArgs e)
        {
            emergencyFightSub(17);
            label70.Text = getNum(17).ToString();
        }

        private void button59_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(18);
            label68.Text = getNum(18).ToString();
        }

        private void button58_Click(object sender, EventArgs e)
        {
            emergencyFightSub(18);
            label68.Text = getNum(18).ToString();
        }

        private void button57_Click(object sender, EventArgs e)
        {
            emergencyFightAdd(19);
            label66.Text = getNum(19).ToString();
        }

        private void button56_Click(object sender, EventArgs e)
        {
            emergencyFightSub(19);
            label66.Text = getNum(19).ToString();
        }

        private void button37_Click(object sender, EventArgs e)
        {
            {
                //5/37/38
                if ((special & 2) != 0)
                {
                    //重复点击则取消
                    button37.BackColor = Color.FromArgb(224, 224, 224);
                    special &= (~2);
                    Refresh_Special();
                    return;
                }

                if ((special & 5) != 0)
                {
                    special &= (~5);
                    //23不能和1同时存在，因此要先清空23位
                    button5.BackColor = Color.FromArgb(224, 224, 224);
                    button38.BackColor = Color.FromArgb(224, 224, 224);
                }
                special |= 2;
                button37.BackColor = Color.Pink;
                Refresh_Special();
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            {
                //5/37/38
                if ((special & 4) != 0)
                {
                    //重复点击则取消
                    button38.BackColor = Color.FromArgb(224, 224, 224);
                    special &= (~4);
                    Refresh_Special();
                    return;
                }

                if ((special & 3) != 0)
                {
                    special &= (~3);
                    //23不能和1同时存在，因此要先清空23位
                    button5.BackColor = Color.FromArgb(224, 224, 224);
                    button37.BackColor = Color.FromArgb(224, 224, 224);
                }
                special |= 4;
                button38.BackColor = Color.Pink;
                Refresh_Special();
            }
        }

        private void button80_Click(object sender, EventArgs e)
        {
            if ((special & 8) != 0)
            {
                //重复点击则取消
                button80.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~8);
                Refresh_Special();
                return;
            }
            special |= 8;
            button80.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button79_Click(object sender, EventArgs e)
        {
            if ((special & 16) != 0)
            {
                //重复点击则取消
                button79.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~16);
                Refresh_Special();
                return;
            }
            special |= 16;
            button79.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button70_Click(object sender, EventArgs e)
        {
            if ((special & 64) != 0)
            {
                //重复点击则取消
                button70.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~64);
                Refresh_Special();
                return;
            }
            special |= 64;
            button70.BackColor = Color.Pink;
            Refresh_Special();

        }

        private void button73_Click(object sender, EventArgs e)
        {
            if ((special & 128) != 0)
            {
                //重复点击则取消
                button73.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~128);
                Refresh_Special();
                return;
            }

            if ((special & 256) != 0)
            {
                special &= (~256);
                button77.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 128;
            button73.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button77_Click(object sender, EventArgs e)
        {
            if ((special & 256) != 0)
            {
                //重复点击则取消
                button77.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~256);
                Refresh_Special();
                return;
            }

            if ((special & 128) != 0)
            {
                special &= (~128);
                button73.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 256;
            button77.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button72_Click(object sender, EventArgs e)
        {
            heroKill++;
            if(heroKill == 6)
            {
                heroKill = 0;
                special ^= 256;
                special |= 128;
                button77.BackColor = Color.FromArgb(224, 224, 224);
                button73.BackColor = Color.Pink;
            }
            Refresh_Special();
        }

        private void button69_Click(object sender, EventArgs e)
        {
            heroKill--;
            if(heroKill < 0)
            {
                heroKill = 0;
            }
            Refresh_Special();
        }

        private void button74_Click(object sender, EventArgs e)
        {
            if ((special & 512) != 0)
            {
                //重复点击则取消
                button74.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~512);
                Refresh_Special();
                return;
            }

            if ((special & 1024) != 0)
            {
                special &= (~1024);
                button78.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 512;
            button74.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button78_Click(object sender, EventArgs e)
        {
            if ((special & 1024) != 0)
            {
                //重复点击则取消
                button78.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~1024);
                Refresh_Special();
                return;
            }

            if ((special & 512) != 0)
            {
                special &= (~512);
                button74.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 1024;
            button78.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button76_Click(object sender, EventArgs e)
        {
            justiceKill++;
            if (justiceKill > 3)
            {
                justiceKill = 3;
            }
            Refresh_Special();
        }

        private void button75_Click(object sender, EventArgs e)
        {
            justiceKill--;
            if (justiceKill < 0) 
            {
                justiceKill = 0;
            }
            Refresh_Special();
        }

        private void button43_Click(object sender, EventArgs e)
        {
            if ((special & 2048) != 0)
            {
                //重复点击则取消
                button43.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~2048);
                Refresh_Special();
                return;
            }
            special |= 2048;
            button43.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            bool success = int.TryParse(textBox6.Text, out startMoney);
            if (success)
            {
                Money_Change();
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            bool success = int.TryParse(textBox7.Text, out endMoney);
            if (success)
            {
                Money_Change();
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            bool success = int.TryParse(textBox8.Text, out board);
            if (success)
            {
                Refresh_Board();
            }
        }

        private void button31_Click_1(object sender, EventArgs e)
        {
            if ((end & 512) != 0)
            {
                //重复点击则取消
                button31.BackColor = Color.FromArgb(224, 224, 224);
                end &= (~512);
                Refresh_End();
                return;
            }
            end |= 512;
            button31.BackColor = Color.Pink;
            Refresh_End();

        }

        private void button44_Click(object sender, EventArgs e)
        {
            if ((special & 4096) != 0)
            {
                //重复点击则取消
                button44.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~4096);
                Refresh_Special();
                return;
            }
            special |= 4096;
            button44.BackColor = Color.Pink;
            Refresh_Special();
        }
    }
}
