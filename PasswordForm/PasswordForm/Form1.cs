using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordForm
{
    public partial class Form1 : Form
    {
        char[,] keyList; // 암호키를 중복제거하고 차례대로 넣어놓을 5*5 이차원 배열
        List<char> senList; // 암호문 변환하는 char 리스트
        private PasswordEngine passwordEngine; // 키 변환, 암호화, 복호화가 진행되는 클래스
        public Form1()
        {
            InitializeComponent();
            passwordEngine = new PasswordEngine(); // 클래스 생성
        }

        private void button1_Click(object sender, EventArgs e)  // 암호화 버튼을 클릭 시
        {
            bool check = passwordEngine.CheckInfo(keyword.Text, sentence.Text); // bool 변수에 체크함수의 결과값 넣기
            if (check == true) // true이면 암호문과 키워드가 다 있으므로
            {
                keyList = passwordEngine.keyMethod(keyword.Text); // keyMethod에 키워드를 넘겨 변환된 키를 넣어놓는 부분
                senList = passwordEngine.senMethod(sentence.Text); // senMethod에 암호할 문장을 넘겨 변환된 문장을 넣어놓는 부분
                String sen = new String(passwordEngine.doSubstitution(keyList, senList, "encryption")); // encryption 단어, 키, 문장을 넘겨 암호화를 한 문장을 담는 부분
                password.Text = sen; // 텍스트박스에 보여주기
            }
            // 키워드나 암호문이 없으니 메시지 박스 띄워주기
            else
            {
                MessageBox.Show("키워드와 암호문을 입력하세요!");
            }
        }

        private void button2_Click(object sender, EventArgs e) // 복호화 버튼을 클릭 시
        {
            bool check = passwordEngine.CheckInfo(keyword.Text, sentence.Text);
            if (check == true) // 값이 있으면
            {
                keyList = passwordEngine.keyMethod(keyword.Text);
                senList = passwordEngine.senMethod(sentence.Text); // senMethod에 복호화할 문장을 넘겨 변환된 문장을 넣어놓는 부분
                String sen = new String(passwordEngine.doSubstitution(keyList, senList, "decryption")); // decryption 단어, 키, 문장을 넘겨 암호화를 한 문장을 담는 부분
                password.Text = sen;
            }
            else // 값이 없으면
            {
                MessageBox.Show("키워드와 암호문을 입력하세요!");
            }
        }
    }
}
