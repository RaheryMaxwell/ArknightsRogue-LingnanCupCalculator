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
using System.Windows.Forms.VisualStyles;
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
        public int dog = 0;
        public int mouse = 0;
        //这四个是隐藏怪的数量

        public int moneyBonus = 0;
        //取钱次数

        public int end = 0;
        /*结局，使用位运算判断
        *1-憧憬未来、2-双王记、4-天使之城
        */

        public bool isGetHatred = false; //是否拥有死仇时代的恨意
        public int hatredEmerg = 0;
        public bool isEW = false; //是否使用了EW
        public bool isMagic = false;//是否使用术特分队
        public bool isBluePrint = false;//是否使用蓝图分队。
        public int special = 0;
        /*
         * 特殊事件，使用位运算判断
         * 1-赴敌者、2-或然面纱、4-离歌的庭院
         * 8-死仇鸭本无漏、16-死仇鸭本仅通关
         * 32-鸭本无漏、64-鸭本仅通关
         * 128-死仇战场侧面无漏、256-死仇战场侧面仅通关
         * 512-死仇熊关无漏、1024-死仇熊关仅通关、2048-熊关无漏
         * 4096-四层以上鸭关无漏
         * 8192-白糖
         */
        public int heroKill = 0;//英雄无名仅通关击杀数
        public int justiceKill = 0;//正义使者仅通关击杀数

        public double[] specialBonus = { 40, 50, 50, 150, 80, 130, 50, 80, 50, 100, 50, 30, 80, 250};
        private void Refresh_Special()
        {
            if (isGetHatred == false)
            {
                //仅有拥有死仇时代的恨意的时候，此项生效。
                hatredEmerg = 0;
            }
            label20.Text = hatredEmerg.ToString();

            double bonus = 0;
            for(int i = 0; i < 14; i++)
            {
                if((special & (1 << i)) != 0)
                {
                    bonus += specialBonus[i];
                }
            }
            label80.Text = bonus.ToString();
            Refresh_All_Bonus();
        }

        //每一个,低16位为分数，高16位为个数
        public int[] emergencyFight = { 30, 60, 80, 100, 90, 80, 40, 120, 120, 160, 180, 90, 50, 70, 70, 120, 120, 140, 70, 150 };
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

            if(moneyBonus < 0)
            {
                moneyBonus = 0;
            }
            label92.Text = moneyBonus.ToString();

            int moneyDefine = 0;
            if (moneyBonus > 10)
            {
                moneyDefine = -50 * (moneyBonus - 10);
            }
            label88.Text = moneyDefine.ToString();
            Refresh_All_Bonus();
        }

        public int collection = 0;
        public int board = 0;
        public int endBonus = 0;

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
            double temp = Double.Parse(label62.Text) + Double.Parse(label63.Text) + Double.Parse(label64.Text) + Double.Parse(label88.Text)
                + Double.Parse(label65.Text) + Double.Parse(label80.Text) +
                + Double.Parse(textBox4.Text) - 30*Double.Parse(label20.Text);
            if (isEW == true)
            {
                temp *= 0.8;
            }
            if(isMagic == true)
            {
                temp *= 0.9;
            }
            if (isBluePrint == true)
            {
                temp = 0;
            }

            label93.Text = temp.ToString();
        }


        private void Refresh_TemporaryRecruitment()
        {
            label8.Text = temporaryRecruitmentSix.ToString();
            label25.Text = temporaryRecruitmentFive.ToString();
            label27.Text = temporaryRecruitmentFour.ToString();
            int temporaryRecruitmentBonus = 10 * temporaryRecruitmentFour + 20 * temporaryRecruitmentFive + 30 * temporaryRecruitmentSix;
            label62.Text = temporaryRecruitmentBonus.ToString();
            Refresh_All_Bonus();
        }

        private void Refresh_DuckBearDogMouse()
        {
            label10.Text = dog.ToString();
            label14.Text = duck.ToString();
            label12.Text = bear.ToString();
            label32.Text = mouse.ToString();
            int bonus = (duck + bear + dog + mouse)*10;
            label64.Text = bonus.ToString();
            Refresh_All_Bonus();
        }

        public int[] bossBonus = { 0, 150, 120 };
        private void Refresh_End()
        {
            int endBonus = 0;
            for(int i = 0; i < 3; i++)
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

            if((end & 2) != 0)
            {
                end &= (~2);
                //234不能和1同时存在，因此要先清空234位
                button2.BackColor = Color.FromArgb(224, 224, 224);
                button33.BackColor = Color.FromArgb(224, 224, 224);
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
            end |= 4;
            button2.BackColor = Color.Pink;
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
            button33.BackColor = Color.FromArgb(224, 224, 224);
            Refresh_End();

            duck = 0;
            bear = 0;
            dog = 0;
            mouse = 0;
            Refresh_DuckBearDogMouse();

            isGetHatred = false;
            isBluePrint = false;
            isEW = false;
            isMagic = false;
            for(int i = 0; i < emergencyFight.Length; i++)
            {
                emergencyFight[i] &= 65535;
            }
            label20.Text = "0";
            label35.Text = "0";
            label33.Text = "0";
            label40.Text = "0";
            label38.Text = "0";
            label15.Text = "0";
            label53.Text = "0";
            label51.Text = "0";
            label49.Text = "0";
            label46.Text = "0";
            emergencyFightRefresh();

            special = 0;
            button3.BackColor = Color.FromArgb(224, 224, 224);
            button4.BackColor = Color.FromArgb(224, 224, 224);
            button5.BackColor = Color.FromArgb(224, 224, 224);
            button16.BackColor = Color.FromArgb(224, 224, 224);
            button17.BackColor = Color.FromArgb(224, 224, 224);
            button37.BackColor = Color.FromArgb(224, 224, 224);
            button38.BackColor = Color.FromArgb(224, 224, 224);
            button44.BackColor = Color.FromArgb(224, 224, 224);
            button80.BackColor = Color.FromArgb(224, 224, 224);
            button79.BackColor = Color.FromArgb(224, 224, 224);
            button70.BackColor = Color.FromArgb(224, 224, 224);
            button43.BackColor = Color.FromArgb(224, 224, 224);
            button28.BackColor = Color.FromArgb(224, 224, 224);
            button29.BackColor = Color.FromArgb(224, 224, 224);
            button30.BackColor = Color.FromArgb(224, 224, 224);
            button31.BackColor = Color.FromArgb(224, 224, 224);
            button70.BackColor = Color.FromArgb(224, 224, 224);
            button43.BackColor = Color.FromArgb(224, 224, 224);
            button34.BackColor = Color.FromArgb(224, 224, 224);
            button35.BackColor = Color.FromArgb(224, 224, 224);
            Refresh_Special();

            moneyBonus = 0;
            Money_Change();

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

            if ((end & 1) != 0)
            {
                end &= (~1);
                button1.BackColor = Color.FromArgb(224,224,224);
                button2.BackColor = Color.FromArgb(224, 224, 224);
            }
            end |= 2;
            button33.BackColor = Color.Pink;
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
            Refresh_DuckBearDogMouse();
        }

        private void button12_Click_1(object sender, EventArgs e)
        {
            duck--;
            if(duck < 0)
            {
                duck = 0;
            }
            Refresh_DuckBearDogMouse();
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            bear++;
            Refresh_DuckBearDogMouse();
        }

        private void button10_Click_1(object sender, EventArgs e)
        {
            bear--;
            if(bear < 0)
            {
                bear = 0;
            }
            Refresh_DuckBearDogMouse(); 
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            dog++;
            Refresh_DuckBearDogMouse();
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            dog--;
            if(dog < 0)
            {
                dog = 0;
            }
            Refresh_DuckBearDogMouse();
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
            if ((special & 16) != 0)
            {
                //重复点击则取消
                button80.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~16);
                Refresh_Special();
                return;
            }
            if ((special & 104) != 0)
            {
                special &= (~104);
                button28.BackColor = Color.FromArgb(224, 224, 224);
                button29.BackColor = Color.FromArgb(224, 224, 224);
                button79.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 16;
            button80.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button79_Click(object sender, EventArgs e)
        {
            if ((special & 64) != 0)
            {
                //重复点击则取消
                button79.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~64);
                Refresh_Special();
                return;
            }
            if ((special & 56) != 0)
            {
                special &= (~56);
                button29.BackColor = Color.FromArgb(224, 224, 224);
                button28.BackColor = Color.FromArgb(224, 224, 224);
                button80.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 64;
            button79.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button70_Click(object sender, EventArgs e)
        {
            if ((special & 512) != 0)
            {
                //重复点击则取消
                button70.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~512);
                Refresh_Special();
                return;
            }
            if ((special & 3072) != 0)
            {
                special &= (~3072);
                button43.BackColor = Color.FromArgb(224, 224, 224);
                button34.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 512;
            button70.BackColor = Color.Pink;
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


        private void button43_Click(object sender, EventArgs e)
        {
            if ((special & 1024) != 0)
            {
                //重复点击则取消
                button43.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~1024);
                Refresh_Special();
                return;
            }
            if ((special & 2560) != 0)
            {
                special &= (~2560);
                button70.BackColor = Color.FromArgb(224, 224, 224);
                button34.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 1024;
            button43.BackColor = Color.Pink;
            Refresh_Special();
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

        private void button81_Click(object sender, EventArgs e)
        {

            MessageBox.Show("明日方舟-萨卡兹肉鸽计分器，使用的规则为岭南杯计分方式，感谢所有为该规则付出的人。\n\n欢迎加入广东高校联合群：813804119，记得标注学校\n", "鸣谢");
        }

        private void label91_Click(object sender, EventArgs e)
        {

        }

        private void label91_Click_1(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void button83_Click(object sender, EventArgs e)
        {
            mouse++;
            Refresh_DuckBearDogMouse(); 
        }

        private void label32_Click_1(object sender, EventArgs e)
        {

        }

        private void button82_Click(object sender, EventArgs e)
        {
            mouse--;
            if (mouse < 0)
            {
                mouse = 0;
            }
            Refresh_DuckBearDogMouse();
        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label92_Click_1(object sender, EventArgs e)
        {

        }

        private void button85_Click(object sender, EventArgs e)
        {
            moneyBonus++;
            Money_Change();
        }

        private void button84_Click(object sender, EventArgs e)
        {
            moneyBonus--;
            Money_Change();
        }

        private void label94_Click(object sender, EventArgs e)
        {

        }

        private void label88_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(isGetHatred == true)
            {
                button3.BackColor = Color.FromArgb(224, 224, 224);
                isGetHatred = false;
                Refresh_Special();
                return;
            }
            isGetHatred = true;
            button3.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            hatredEmerg++;
            Refresh_Special();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            hatredEmerg--;
            if (hatredEmerg < 0)
            {
                hatredEmerg = 0;
            }
            Refresh_Special();
        }

        private void label20_Click_1(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (isEW == true)
            {
                button4.BackColor = Color.FromArgb(224, 224, 224);
                isEW = false;
                Refresh_Special();
                return;
            }
            isEW = true;
            button4.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (isBluePrint == true)
            {
                button17.BackColor = Color.FromArgb(224, 224, 224);
                isBluePrint = false;
            }

            if (isMagic == true)
            {
                button16.BackColor = Color.FromArgb(224, 224, 224);
                isMagic = false;
                Refresh_Special();
                return;
            }

            isMagic = true;
            button16.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (isMagic == true)
            {
                button16.BackColor = Color.FromArgb(224, 224, 224);
                isMagic = false;
            }

            if (isBluePrint == true)
            {
                button17.BackColor = Color.FromArgb(224, 224, 224);
                isBluePrint = false;
                Refresh_Special();
                return;
            }


            isBluePrint = true;
            button17.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if ((special & 256) != 0)
            {
                //重复点击则取消
                button31.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~256);
                Refresh_Special();
                return;
            }
            if ((special & 128) != 0)
            {
                special &= (~128);
                button30.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 256;
            button31.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if ((special & 128) != 0)
            {
                //重复点击则取消
                button30.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~128);
                Refresh_Special();
                return;
            }
            if ((special & 256) != 0)
            {
                special &= (~256);
                button31.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 128;
            button30.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if ((special & 8) != 0)
            {
                //重复点击则取消
                button28.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~8);
                Refresh_Special();
                return;
            }
            if ((special & 112) != 0)
            {
                special &= (~112);
                button80.BackColor = Color.FromArgb(224, 224, 224);
                button29.BackColor = Color.FromArgb(224, 224, 224);
                button79.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 8;
            button28.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void label80_Click(object sender, EventArgs e)
        {

        }

        private void button29_Click_1(object sender, EventArgs e)
        {
            if ((special & 32) != 0)
            {
                //重复点击则取消
                button29.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~32);
                Refresh_Special();
                return;
            }
            if ((special & 88) != 0)
            {
                special &= (~88);
                button79.BackColor = Color.FromArgb(224, 224, 224);
                button28.BackColor = Color.FromArgb(224, 224, 224);
                button80.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 32;
            button29.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button34_Click(object sender, EventArgs e)
        {
            if ((special & 2048) != 0)
            {
                //重复点击则取消
                button34.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~2048);
                Refresh_Special();
                return;
            }
            if ((special & 1536) != 0)
            {
                special &= (~1536);
                button70.BackColor = Color.FromArgb(224, 224, 224);
                button43.BackColor = Color.FromArgb(224, 224, 224);
            }
            special |= 2048;
            button34.BackColor = Color.Pink;
            Refresh_Special();
        }

        private void button35_Click(object sender, EventArgs e)
        {
            if ((special & 4096) != 0)
            {
                //重复点击则取消
                button35.BackColor = Color.FromArgb(224, 224, 224);
                special &= (~4096);
                Refresh_Special();
                return;
            }
            special |= 4096;
            button35.BackColor = Color.Pink;
            Refresh_Special();
        }
    }
}
