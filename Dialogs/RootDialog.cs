using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Text.RegularExpressions;

namespace CalcChatBot.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            char[] op = { '+', '-', '*', '/', '%' };
            string text = activity.Text ?? string.Empty; //유저가 보낸 메세지
            string[] operand = text.Split(op); //피연산자를 담는다

            if(operand.Length>2)
            {
                await context.PostAsync($"인수가 너무 많습니다. 형식: N op N");
            }
            else
            {
                string pattern = @"[^+*/%\-]"; //정규표현식
                Regex rgx = new Regex(pattern);
                string[] opcodeArray = rgx.Split(text); //연산자를 담는다 
                double calcReult = 0;
                double[] a = new double[operand.Length];
                string opcode=null;

                for (int i = 0; i < operand.Length; i++)
                {
                    a[i]=Convert.ToDouble(operand[i]);
                    //await context.PostAsync($"{i} 번째 수 : {a[i]}"); 디버깅
                }

                for (int i = 0; i < opcodeArray.Length; i++)
                {
                    if(opcodeArray[i]=="+"|| opcodeArray[i] == "-"|| opcodeArray[i] == "*"|| opcodeArray[i] == "/"|| opcodeArray[i] == "%")
                    {
                        opcode = opcodeArray[i];
                    }
                    //await context.PostAsync($"{i} 연산자 : {opcodeArray[i]}"); 디버깅
                }
                //await context.PostAsync($"연산자 : {opcode}"); 디버깅

                if (opcode=="+")
                {
                    calcReult = a[0] + a[1];
                }
                else if (opcode == "-")
                {
                    calcReult = a[0] - a[1];
                }
                else if (opcode == "*")
                {
                    calcReult = a[0] * a[1];
                }
                else if (opcode == "/")
                {
                    calcReult = a[0] / a[1];
                }
                else if (opcode == "%")
                {
                    calcReult = a[0] % a[1];
                }
                else
                {
                    calcReult = 0;
                }

                await context.PostAsync($"재밌는 산수다!!");
                await context.PostAsync($"계산결과 : {calcReult}");
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}