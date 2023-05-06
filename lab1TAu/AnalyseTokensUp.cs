using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static lab1TAu.Token;

namespace lab1TAu
{
    public class AnalyseTokensUp
    {
        List<Token> tokens = new List<Token>();
        Stack<Token> lexemStack = new Stack<Token>();
        Stack<int> stateStack = new Stack<int>();
        int nextLex = 0;
        int state = 0;
        bool isEnd = false;
        public AnalyseTokensUp(List<Token> vvodtoken)
        {
            tokens = vvodtoken;
        }
        public void Error(Token.TokenType Ozhidal, Token.TokenType Poluch)
        {
            throw new Exception($"Ожидалось: {Ozhidal}, а получено: {Poluch}");
        }
        void Shift()
        {
            lexemStack.Push(GetLexeme(nextLex));
            nextLex++;
        }
        private Token GetLexeme(int nextLex)
        {
            return tokens[nextLex];
        }
        private void Reduce(int num, string neterm)
        {
            for (int i = 0; i < num; i++)
            {
                lexemStack.Pop();
                stateStack.Pop();
            }
            state = stateStack.Peek();
            Token k = new Token(TokenType.NETERM);
            k.Value = neterm;
            lexemStack.Push(k);
        }
        void GoToState(int state)
        {
            stateStack.Push(state);
            this.state = state;
        }
        void State0()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<программа>":
                            if (nextLex == tokens.Count)
                                isEnd = true;
                            break;
                        case "<спис.объяв>":
                            GoToState(1);
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.DIM:
                    GoToState(2);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State1() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис.опер>":
                            GoToState(3);
                            break;
                        case "<опер>":
                            GoToState(4);
                            break;
                        case "<присвоен>":
                            GoToState(17);
                            break;
                        case "<условн>":
                            GoToState(18);
                            break;
                        case "<спис.объяв>":
                            Shift();
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(19);
                    break;
                case TokenType.IF:
                    GoToState(20);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State2() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис.идент>":
                            GoToState(5);
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(19);
                    break;
                case TokenType.DIM:
                    Shift();
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State3() 
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<спис.опер>")
                Reduce(1, "<программа>");
            else
                throw new Exception($"Ожидалось правило <спис.опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State4() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опер>":
                            Shift();
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.ENTER:
                    GoToState(15);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State5() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис.идент>":
                            Shift();
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.AS:
                    GoToState(7);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State6() 
        { 
        } 
        void State7() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип>":
                            GoToState(8);
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.AS:
                    Shift();
                    break;
                case TokenType.INTEGER:
                    GoToState(9);
                    break;
                case TokenType.STRING:
                    GoToState(11);
                    break;
                case TokenType.BOOL:
                    GoToState(10);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State8() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип>":
                            Shift();
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.ENTER:
                    GoToState(12);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State9() 
        {
            if (lexemStack.Peek().Type == TokenType.INTEGER)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Ожидалась лексема integer, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State10() 
        {
            if (lexemStack.Peek().Type == TokenType.BOOL)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Ожидалась лексема bool, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }  
        void State11() 
        {
            if (lexemStack.Peek().Type == TokenType.STRING)
                Reduce(1, "<тип>");
            else
                throw new Exception($"Ожидалась лексема string, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        } 
        void State12() 
        {
            if (lexemStack.Peek().Type == TokenType.ENTER)
                Reduce(5, "<спис.объяв>");
            else
                throw new Exception($"Ожидалась лексема ENTER, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State13() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис.идент>":
                            GoToState(14);
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(6);
                    break;
                case TokenType.COMMA:
                    Shift();
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State14() 
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<спис.идент>")
                Reduce(3, "<спис.объяв>");
            else
                throw new Exception($"Ожидалась лексема ENTER, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State15() { }
        void State16()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<спис.опер>")
                Reduce(3, "<опер>");
            else
                throw new Exception($"Ожидалось правило <спис.опер>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State17()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<присвоен>")
                Reduce(1, "<опер>");
            else
                throw new Exception($"Ожидалась правило <присвоен>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State18()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<условн>")
                Reduce(1, "<опер>");
            else
                throw new Exception($"Ожидалась правило <условн>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State19() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.IDENTIFIER:
                    Shift();
                    break;
                case TokenType.EQUAL:
                    GoToState(21);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State20() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<слож.операнд>":
                            GoToState(23);
                            break;
                        case "<операнд>":
                            GoToState(25);
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.IF:
                    Shift();
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(26);
                    break;
                case TokenType.LITERAL:
                    GoToState(27);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State21() { }
        void State22() { }
        void State23() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<слож.операнд>":
                            Shift();
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.THEN:
                    GoToState(24);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State24() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.THEN:
                    Shift();
                    break;
                case TokenType.ENTER:
                    GoToState(36);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State25() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<операнд>":
                            Shift();
                            break;
                        case "<знак>":
                            GoToState(28);
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.PLUS:
                    GoToState(29);
                    break;
                case TokenType.MINUS:
                    GoToState(30);
                    break;
                case TokenType.MULTIPLY:
                    GoToState(31);
                    break;
                case TokenType.DIVISION:
                    GoToState(32);
                    break;
                case TokenType.MORE:
                    GoToState(33);
                    break;
                case TokenType.LESS:
                    GoToState(34);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State26() 
        {
            if (lexemStack.Peek().Type == TokenType.IDENTIFIER)
                Reduce(1, "<слож.операнд>");
            else
                throw new Exception($"Ожидался терминал IDENTIFIER, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State27() 
        {
            if (lexemStack.Peek().Type == TokenType.LITERAL)
                Reduce(1, "<слож.операнд>");
            else
                throw new Exception($"Ожидался терминал LITERAL, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State28() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<знак>":
                            Shift();
                            break;
                        case "<операнд>":
                            GoToState(35);
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(26);
                    break;
                case TokenType.LITERAL:
                    GoToState(27);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State29() 
        {
            if (lexemStack.Peek().Type == TokenType.PLUS)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Ожидался терминал PLUS, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State30() 
        {
            if (lexemStack.Peek().Type == TokenType.MINUS)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Ожидался терминал MINUS, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
            
        void State31() 
        {
            if (lexemStack.Peek().Type == TokenType.MULTIPLY)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Ожидался терминал MULTIPLY, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State32() 
        {
            if (lexemStack.Peek().Type == TokenType.DIVISION)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Ожидался терминал DIVISION, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State33() 
        {
            if (lexemStack.Peek().Type == TokenType.LESS)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Ожидался терминал LESS, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State34() 
        {
            if (lexemStack.Peek().Type == TokenType.MORE)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Ожидался терминал MORE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State35() 
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<операнд>")
                Reduce(3, "<условн>");
            else
                throw new Exception($"Ожидалась правило <операнд>, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State36() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис.опер>":
                            GoToState(37);
                            break;
                        case "<опер>":
                            GoToState(4);
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.ENTER:
                    Shift();
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State37() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис.опер>":
                            Shift();
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.END:
                    GoToState(38);
                    break;
                case TokenType.ELSE:
                    GoToState(39);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State38()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.END:
                    Shift();
                    break;
                case TokenType.IF:
                    GoToState(40);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State39() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.ELSE:
                    Shift();
                    break;
                case TokenType.ENTER:
                    GoToState(41);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State40()
        {
            if (lexemStack.Peek().Type == TokenType.IF)
                Reduce(7, "<опер>");
            else
                throw new Exception($"Ожидался терминал IF, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State41() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис.опер>":
                            GoToState(42);
                            break;
                        case "<опер>":
                            GoToState(4);
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.ENTER:
                    Shift();
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State42() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис.опер>":
                            Shift();
                            break;
                        default:
                            Error();
                            break;
                    }
                    break;
                case TokenType.END:
                    GoToState(43);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State43() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.END:
                    Shift();
                    break;
                case TokenType.IF:
                    GoToState(44);
                    break;
                default:
                    Error();
                    break;
            }
        }
        void State44() 
        {
            if (lexemStack.Peek().Type == TokenType.IF)
                Reduce(10, "<опер>");
            else
                throw new Exception($"Ожидался терминал IF, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }

    }
}
