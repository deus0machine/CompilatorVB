using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static lab1TAu.Token;

namespace lab1TAu
{
    public struct Three
    {
        public Token operand1;
        public Token operand2;
        public Token znak;
        public Three(Token dy, Token op2, Token op1)
        {
            operand1 = op1;
            operand2 = op2;
            znak = dy;
        }
    }
    public class ExpressionAnalyse
    {
        int index = 0;
        public List<Three> troyka = new List<Three>();
        List<Token> tokens = new List<Token>();
        Stack<Token> E = new Stack<Token>();
        Stack<Token> T = new Stack<Token>();
        int nextlex = 0;
        public ExpressionAnalyse(List<Token> inmet)
        {
            tokens = inmet;
        }
        private Token GetLexeme(int nextLex)
        {
            return tokens[nextLex];
        }
        private void NextId()
        {
            E.Push(tokens[nextlex]);
            nextlex++;
        }
        private void CreateThree()
        {
            Three k = new Three(T.Pop(), E.Pop(), E.Pop());
            troyka.Add(k);
            Token l = new Token(TokenType.IDENTIFIER);
            l.Value = $"m{index}";
            E.Push(l);
            index++;
        }
        public void Start()
        {
            if (nextlex == tokens.Count)
            {
                if (T.Count == 0)
                    return;
                else
                {
                    EndList();
                }
            }
            else
            {
                switch (GetLexeme(nextlex).Type)
                {
                    case TokenType.IDENTIFIER:
                        NextId();
                        break;
                    case TokenType.LITERAL:
                        NextId();
                        break;
                    case TokenType.PLUS:
                        PlusMinusOr();
                        break;
                    case TokenType.MINUS:
                        PlusMinusOr();
                        break;
                    case TokenType.MULTIPLY:
                        MultiplyDivideAnd();
                        break;
                    case TokenType.DIVISION:
                        MultiplyDivideAnd();
                        break;
                    case TokenType.LPAR:
                        Lpar();
                        break;
                    case TokenType.RPAR:
                        Rpar();
                        break;
                    default:
                        Error("+, -, *, /, (, ), идентификатор или литерал");
                break;
                }
            }
            Start();
        }
        private void MultiplyDivideAnd()
        {
            if (T.Count == 0)
                D1();
            else
                switch (T.Peek().Type)
                {
                    case TokenType.LPAR:
                        D1();
                        break;
                    case TokenType.PLUS:
                        D1();
                        break;
                    case TokenType.MINUS:
                        D1();
                        break;
                    case TokenType.MULTIPLY:
                        D2();
                        break;
                    case TokenType.DIVISION:
                        D2();
                        break;
                    default:
                        Error("+, -, *, /, ( или )");
                        break;
                }
        }
        private void PlusMinusOr()
        {
            if (T.Count == 0)
                D1();
            else
                switch (T.Peek().Type)
                {
                    case TokenType.LPAR:
                        D1();
                        break;
                    case TokenType.PLUS:
                        D2();
                        break;
                    case TokenType.MINUS:
                        D2();
                        break;
                    case TokenType.MULTIPLY:
                        D4();
                        break;
                    case TokenType.DIVISION:
                        D4();
                        break;
                    default:
                        Error("+, -, *, /, ( или )");
                        break;
                }
        }
        private void Lpar()
        {
            {
                if (T.Count == 0)
                    D1();
                else
                    switch (T.Peek().Type)
                    {
                        case TokenType.LPAR:
                            D1();
                            break;
                        case TokenType.PLUS:
                            D1();
                            break;
                        case TokenType.MINUS:
                            D1();
                            break;
                        case TokenType.MULTIPLY:
                            D1();
                            break;
                        case TokenType.DIVISION:
                            D1();
                            break;
                        default:
                            Error("+, -, *, /, ( или )");
                            break;
                    }
            }
        }
        private void Rpar()
        {
            {
                if (T.Count == 0)
                    D5();
                else
                    switch (T.Peek().Type)
                    {
                        case TokenType.LPAR:
                            D3();
                            break;
                        case TokenType.PLUS:
                            D4();
                            break;
                        case TokenType.MINUS:
                            D4();
                            break;
                        case TokenType.MULTIPLY:
                            D4();
                            break;
                        case TokenType.DIVISION:
                            D4();
                            break;
                        default:
                            Error("+, -, *, /, ( или )");
                            break;
                    }
            }
        }
        private void EndList()
        {
            {
                if (T.Count == 0)
                    D5();
                else
                    switch (T.Peek().Type)
                    {
                        case TokenType.LPAR:
                            D5();
                            break;
                        case TokenType.PLUS:
                            D4();
                            break;
                        case TokenType.MINUS:
                            D4();
                            break;
                        case TokenType.MULTIPLY:
                            D4();
                            break;
                        case TokenType.DIVISION:
                            D4();
                            break;
                        default:
                            Error("+, -, *, /, ( или )");
                            break;
                    }
            }
        }
        private void D1()
        {
            T.Push(tokens[nextlex]);
            nextlex++;
        }
        private void D2()
        {
            CreateThree();
            T.Push(tokens[nextlex]);
            nextlex++;
        }
        private void D3()
        {
            T.Pop();
            nextlex++;
        }
        private void D4()
        {
            CreateThree();
        }
        private void D5()
        {
            throw new Exception("Ошибка в выражении. Конец разбора");
        }
        private void Error(string ojid)
        {
            if (tokens[nextlex].Type == TokenType.NETERM)
                throw new Exception($"Ожидалось {ojid}, но было получено{tokens[nextlex].Value}");
            /*else
                throw new Exception($"Ожидалось {ojid}, но было получено{LR.ConvertLex(tokens[nextlex].Type)}");*/
        }


    }
}
