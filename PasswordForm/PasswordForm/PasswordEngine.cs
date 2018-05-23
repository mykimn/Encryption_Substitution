using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PasswordForm
{

    class PasswordEngine
    {
        internal char[,] keyMethod(string key) // 키워드의 변환을 담당하는 함수, 매개변수로 key받고, 이차원 char배열을 리턴
        {
            key = key.ToLower(); // 소문자로 변환
            List<char> keyword = new List<char>(); // 키워드 담아놓을 리스트
            List<char> keyList = new List<char>(); // 알파벳 포함해서 키워드 담을 리스트
            char[,] keyArray = new char[5, 5]; // 5*5 크기의 이차원 배열
            char[] Key = key.ToCharArray(); // string인 key를 한 글자씩 char 배열에 넣기
            for (int i = 0; i < Key.Length; i++)
            {
                keyword.Add(Key[i]); // 입력받은 키워드의 길이만큼 리스트에 추가
            }

            for (int i = 0; i < 25; i++) 
            {
                keyword.Add((char)('a' + i)); // 알파벳 a부터 차례대로 리스트에 추가
            }

            for (int i = 0; i < keyword.Count; i++)
            {
                if (keyList.Contains(keyword[i])) // 리스트를 비교하며 중복제거
                {
                    continue;
                }
                keyList.Add(keyword[i]); // 중복 아닌 것은 리스트에 추가
            }

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    keyArray[i, j] = keyList[i * 5 + j]; // 5*5 크기에 차례대로 맵핑
                }
            }
            return keyArray; // 5*5 크기의 배열 리턴
        }

        internal List<char> senMethod(string writing) // 암호문의 변환을 담당하는 함수, 매개변수로 writing받고, char리스트를 리턴
        {
            writing = writing.ToLower(); // 소문자로 변환
            writing = Regex.Replace(writing, " ", ""); // 입력받은 암호문의 공백 제거
            char[] sen = writing.ToArray(); // 한 글자씩 char배열에 넣기
            List<char> senList = new List<char>(); // char 리스트 선언
            for (int i = 0; i < sen.Length; i++)
            {
                senList.Add(sen[i]); // 리스트에 char배열의 값 하나씩 추가
            }
            for (int i = 0; i < sen.Length-1; i += 2) // 2자리씩 for문 돌기
            {

                if (sen[i] == sen[i + 1]) // 두 자리씩 했을 때 다음 자리와 문자가 같으면
                {
                    senList.Insert(i + 1, 'x'); // 중간에 x 삽입
                }
            }
            if (senList.Count % 2 != 0) // for문 다 돌고 리스트의 수가 홀수이면
            {
                senList.Add('x'); // 끝에 x 넣기
            }
            return senList; // char리스트 리턴
        }

        internal char[] doSubstitution(char[,] keyArray, List<char> senList, String keyword) // 암호화 및 복호화를 하는 함수, 매개변수로 이차원배열, 리스트, 문자열, char배열 리턴
        {
            char[] enSentence = new char[senList.Count]; // 암호문의 길이만큼 char배열 생성
            int tmp1=0, tmp2=0, tmp3=0, tmp4 = 0; // 이차원 배열의 인덱스를 저장할 임시변수
            if(keyword.Equals("encryption")) // 넘어온 키워드가 encryption이면 암호화 진행
            {
                for (int i = 0; i < senList.Count; i += 2) // 2개씩 나누어
               {
                    if (senList[i] == 'z') senList[i] = 'q'; // 만약 z가 암호문에 있으면 q로 바꿔서 암호화 진행
                    if (senList[i + 1] == 'z') senList[i] = 'q';
                    for (int j = 0; j < 5; j++) { // 이차원 배열 크기만큼 돌며
                       for (int y = 0; y < 5; y++) {
                           if (keyArray[j, y] == senList[i]) { // 암호문의 값과 키 배열의 값과 일치하는 인덱스를 저장 
                                tmp1 = j;
                                tmp2 = y;
                           }
                           if (keyArray[j, y] == senList[i+1]) {
                                tmp3 = j;
                                tmp4 = y;
                           }
                        }
                     }
                     if (tmp1 == tmp3) { // tmp1과 tmp3이 같으면 가로로 같은 줄
                        if (tmp2 == 4)  tmp2 = -1;  // 4의 경우에는 암호화 시 다시 0번으로 돌아가야하므로
                        if (tmp4 == 4)  tmp4 = -1; 
                        enSentence[i] = keyArray[tmp1, tmp2 + 1]; // 오른쪽으로 한칸 씩 이동
                        enSentence[i + 1] = keyArray[tmp3, tmp4 + 1];
                     }
                     else if (tmp2 == tmp4) { // tmp2와 tmp4가 같으면 세로로 같은 줄 
                        if (tmp1 == 4) tmp1 = -1; // 4의 경우에는 암호화 시 다시 0번으로 돌아가야하므로 
                        if (tmp3 == 4)  tmp3 = -1; 
                        enSentence[i] = keyArray[tmp1 + 1, tmp2]; // 밑으로 한칸 이동
                        enSentence[i + 1] = keyArray[tmp3 + 1, tmp4];
                     }
                     else { // 같은 줄에 있는 경우가 아님
                        enSentence[i] = keyArray[tmp3, tmp2];  // 행은 뒤의 문자꺼로, 열은 나의 문자꺼로
                        enSentence[i + 1] = keyArray[tmp1, tmp4];
                     }
                  }
              }
              else { // encryption이 아닌 다른 키워드가 들어온 경우 ex) decryption => 복호화
                for (int i = 0; i < senList.Count; i += 2) {  // 2개씩 나누어
                    if (senList[i] == 'z') senList[i] = 'q'; // 만약 z가 암호문에 있으면 q로 바꿔서 암호화 진행
                    if (senList[i + 1] == 'z') senList[i] = 'q';
                    for (int j = 0; j < 5; j++) { // 이차원 배열 크기만큼 돌며
                        for (int y = 0; y < 5; y++) {
                            if (keyArray[j, y] == senList[i]) { // 암호문의 값과 키 배열의 값과 일치하는 인덱스를 저장
                                tmp1 = j;
                                tmp2 = y;
                            }
                            if (keyArray[j, y] == senList[i + 1]) {
                                tmp3 = j;
                                tmp4 = y;
                            }
                         }
                     }
                     if (tmp1 == tmp3) { // // tmp1과 tmp3이 같으면 가로로 같은 줄
                        if (tmp2 == 0) tmp2 = 5; // 복호화 시 -1을 하므로 0의 경우에는 다시 4로 되돌아 가기 위해
                        if (tmp4 == 0)  tmp4 = 5;  
                        enSentence[i] = keyArray[tmp1, tmp2 - 1]; // 암호화 된 문을 복호화 하기 위해 -1
                        enSentence[i + 1] = keyArray[tmp3, tmp4 - 1];
                     }
                     else if (tmp2 == tmp4) { // tmp2와 tmp4가 같으면 세로로 같은 줄
                        if (tmp3 == 0) tmp3 = 5;
                        if (tmp1 == 0) tmp1 = 5;
                        enSentence[i] = keyArray[tmp1 - 1, tmp2];
                        enSentence[i + 1] = keyArray[tmp3 - 1, tmp4];
                     }
                     else { // 그냥 복호화 진행
                        enSentence[i] = keyArray[tmp3, tmp2];
                        enSentence[i + 1] = keyArray[tmp1, tmp4];
                     }
                 }
             }
             return enSentence; // 암호화 혹은 복호화를 진행한 char배열을 리턴
        }

        internal bool CheckInfo(string p1, string p2) // 암호문과 키워드에 값이 입력되었는지 확인하는 함수
        {
            bool check; // 확인여부
            if (p1.Equals("") || p2.Equals("")) // 내용이 없으면
            {
                check = false; // false
            }
            else
            {
                check = true; // 있으면 true
            }
            return check; // bool 리턴
        }
    }
}
