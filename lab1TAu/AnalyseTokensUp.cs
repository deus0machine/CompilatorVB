using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static lab1TAu.Token;

namespace lab1TAu
{
    public class AnalyseTokensUp
    {
        List<Token> tokens = new List<Token>();
        Stack<Token> lexemStack = new Stack<Token>();
        Stack<int> stateStack = new Stack<int>();
        public List<List<Three>> listtroek = new List<List<Three>>();
        public ExpressionAnalyse exprAn;
        int nextLex = 0;
        int state = 0;
        bool isEnd = false;
        public bool Succes = false;
        public AnalyseTokensUp(List<Token> vvodtoken)
        {
            tokens = vvodtoken;
        }
        public void Error(Token.TokenType Ozhidal, Token.TokenType Poluch)
        {
            throw new Exception($"Ожидалось: {Ozhidal}, а получено: {Poluch} {nextLex} {lexemStack.Peek().Value}");
        }
        void Shift()
        {
            lexemStack.Push(GetLexeme(nextLex));
            nextLex++;
        }
        public string ConvertToken(Token element)
        {
            string res ="";
            switch (element.Type)
            {
                case (TokenType.MINUS):
                    res = "-";
                    break;
                case (TokenType.PLUS):
                    res = "-";
                    break;
                case (TokenType.MULTIPLY):
                    res = "*";
                    break;
                case (TokenType.DIVISION):
                    res = "/";
                    break;
                case (TokenType.MORE):
                    res = ">";
                    break;
                case (TokenType.LESS):
                    res = "<";
                    break;
            }
            return res;
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
        private void Expression()
        {
             List<Token> listExpr = new List<Token>();
            if (lexemStack.Peek().Type == TokenType.ENTER)
            {
                Error(TokenType.LITERAL, lexemStack.Peek().Type);
            }
             while (lexemStack.Peek().Type != TokenType.ENTER)
             {
                 if (lexemStack.Peek().Type == TokenType.IDENTIFIER ||
                     lexemStack.Peek().Type == TokenType.LITERAL ||
                     lexemStack.Peek().Type == TokenType.DIVISION ||
                     lexemStack.Peek().Type == TokenType.LPAR ||
                     lexemStack.Peek().Type == TokenType.RPAR ||
                     lexemStack.Peek().Type == TokenType.MINUS ||
                     lexemStack.Peek().Type == TokenType.PLUS ||
                     lexemStack.Peek().Type == TokenType.MULTIPLY)
                 {
                     listExpr.Add(lexemStack.Pop());
                     Shift();
                 }
                 else
                 {
                     Error(TokenType.MINUS, lexemStack.Peek().Type);
                 }
             }
            exprAn = new ExpressionAnalyse(listExpr);
            exprAn.Start();
            listtroek.Add(exprAn.troyka);
            if (lexemStack.Peek().Type == TokenType.ENTER)
            {
                lexemStack.Pop();
                nextLex--;
            }
            else
            {
                Error(TokenType.ENTER, lexemStack.Peek().Type);
            }
            //nextLex++;
            Token k = new Token(TokenType.EXPR);
            lexemStack.Push(k);
        }
        void State0()
        {
            if (lexemStack.Count == 0)
                Shift();
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
                            break;
                    }
                    break;
                case TokenType.DIM:
                    GoToState(2);
                    break;
                default:
                    Error(TokenType.DIM, lexemStack.Peek().Type);
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
                            if (lexemStack.Count > 2)
                            {
                                Reduce(2, "<спис.опер>");
                                GoToState(1);
                            }
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
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
                    Error(TokenType.IDENTIFIER, lexemStack.Peek().Type);
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
                            break;
                    }
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(6);
                    break;
                case TokenType.DIM:
                    Shift();
                    break;
                default:
                    Error(TokenType.IDENTIFIER, lexemStack.Peek().Type);
                    break;
            }
        }
        void State3() 
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<спис.опер>")
                Reduce(2, "<программа>");
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
                            break;
                    }
                    break;
                case TokenType.ENTER:
                    GoToState(15);
                    break;
                default:
                    Error(TokenType.ENTER, lexemStack.Peek().Type);
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
                            break;
                    }
                    break;
                case TokenType.AS:
                    GoToState(7);
                    break;
                default:
                    Error(TokenType.AS, lexemStack.Peek().Type);
                    break;
            }
        }
        void State6() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.IDENTIFIER:
                    Shift();
                    if (lexemStack.Peek().Type == TokenType.AS)
                    {
                        lexemStack.Pop();
                        Reduce(1, "<спис.идент>");
                        lexemStack.Push(new Token(TokenType.AS));
                        GoToState(5);
                    }
                    break;
                case TokenType.COMMA:
                    GoToState(13);
                    break;
                default:
                    Error(TokenType.IDENTIFIER, lexemStack.Peek().Type);
                    break;
            }
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
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
                    Error(TokenType.INTEGER, lexemStack.Peek().Type);
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
                            break;
                    }
                    break;
                case TokenType.ENTER:
                    GoToState(12);
                    break;
                default:
                    Error(TokenType.ENTER, lexemStack.Peek().Type);
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
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
                    Error(TokenType.IDENTIFIER, lexemStack.Peek().Type);
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
        void State15() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис.опер>":
                            GoToState(16);
                            break;
                        default:
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
                            break;
                    }
                    break;
                case TokenType.ENTER:
                    if (nextLex == tokens.Count)
                    {
                        Reduce(2, "<спис.опер>");
                        break;
                    }
                    if (GetLexeme(nextLex).Type == TokenType.IF || GetLexeme(nextLex).Type == TokenType.IDENTIFIER)
                    {
                        Reduce(2, "<спис.опер>");
                        Shift();
                    }
                    if (GetLexeme(nextLex).Type == TokenType.ELSE || GetLexeme(nextLex).Type == TokenType.END)
                    {
                        Reduce(2, "<спис.опер>");
                    }
                    /*if (lexemStack.Peek().Type == TokenType.IF || lexemStack.Peek().Type == TokenType.IDENTIFIER)
                    {
                        stateStack.Pop();
                        stateStack.Pop();
                        state = stateStack.Peek();
                    }

                    if (lexemStack.Peek().Type == TokenType.ELSE)
                    {
                        lexemStack.Pop();
                        Reduce(2, "<спис.опер>");
                        GoToState(15);
                    }*/
                    break;
                default:
                    Error(TokenType.IDENTIFIER, lexemStack.Peek().Type);
                    break;
            }
        }
        void State16()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<спис.опер>")
                Reduce(3, "<спис.опер>");
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
                    Error(TokenType.EQUAL, lexemStack.Peek().Type);
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
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
                    Error(TokenType.LITERAL, lexemStack.Peek().Type);
                    break;
            }
        }
        void State21() 
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.EQUAL:
                    Shift();
                    Expression();
                    break;
                case TokenType.EXPR:
                    GoToState(22);
                    break;
                default:
                    Error(TokenType.EQUAL, lexemStack.Peek().Type);
                    break;
            }
        }
        void State22() 
        {
            if (lexemStack.Peek().Type == TokenType.EXPR)
                Reduce(3, "<присвоен>");
            else
                Error(TokenType.EXPR, lexemStack.Peek().Type);
        }
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
                            break;
                    }
                    break;
                case TokenType.THEN:
                    GoToState(24);
                    break;
                default:
                    Error(TokenType.THEN, lexemStack.Peek().Type);
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
                    Error(TokenType.ENTER, lexemStack.Peek().Type);
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
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
                    Error(TokenType.PLUS, lexemStack.Peek().Type);
                    break;
            }
        }
        void State26() 
        {
            if (lexemStack.Peek().Type == TokenType.IDENTIFIER)
                Reduce(1, "<операнд>");
            else
                throw new Exception($"Ожидался терминал IDENTIFIER, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State27() 
        {
            if (lexemStack.Peek().Type == TokenType.LITERAL)
                Reduce(1, "<операнд>");
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
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
                    Error(TokenType.LITERAL, lexemStack.Peek().Type);
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
            if (lexemStack.Peek().Type == TokenType.MORE)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Ожидался терминал MORE, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State34() 
        {
            if (lexemStack.Peek().Type == TokenType.LESS)
                Reduce(1, "<знак>");
            else
                throw new Exception($"Ожидался терминал LESS, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        void State35() 
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<операнд>")
                Reduce(3, "<слож.операнд>");
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
                            break;
                    }
                    break;
                case TokenType.ENTER:
                    Shift();
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(19);
                    break;
                case TokenType.IF:
                    GoToState(20);
                    break;
                default:
                    Error(TokenType.ENTER, lexemStack.Peek().Type);
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
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
                    Error(TokenType.ELSE, lexemStack.Peek().Type);
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
                    Error(TokenType.END, lexemStack.Peek().Type);
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
                    Error(TokenType.ENTER, lexemStack.Peek().Type);
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
                        case "<присвоен>":
                            GoToState(17);
                            break;
                        case "<условн>":
                            GoToState(18);
                            break;
                        default:
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
                            break;
                    }
                    break;
                case TokenType.ENTER:
                    Shift();
                    break;
                case TokenType.IDENTIFIER:
                    GoToState(19);
                    break;
                case TokenType.IF:
                    GoToState(20);
                    break;
                default:
                    Error(TokenType.ENTER, lexemStack.Peek().Type);
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
                            Error(TokenType.NETERM, lexemStack.Peek().Type);
                            break;
                    }
                    break;
                case TokenType.END:
                    GoToState(43);
                    break;
                default:
                    Error(TokenType.END, lexemStack.Peek().Type);
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
                    Error(TokenType.IF, lexemStack.Peek().Type);
                    break;
            }
        }
        void State44() 
        {
            if (lexemStack.Peek().Type == TokenType.IF)
            {
                Reduce(10, "<условн>");
            }    
                
            else
                throw new Exception($"Ожидался терминал IF, но было получено {lexemStack.Peek().Type} {lexemStack.Peek().Value}");
        }
        public void Start()
        {
            stateStack.Push(0);
            try
            {
                Succes = false;
                while (isEnd != true)
                {
                    switch (state)
                    {
                        case 0:
                            State0();
                            break;
                        case 1:
                            State1();
                            break;
                        case 2:
                            State2();
                            break;
                        case 3:
                            State3();
                            break;
                        case 4:
                            State4();
                            break;
                        case 5:
                            State5();
                            break;
                        case 6:
                            State6();
                            break;
                        case 7:
                            State7();
                            break;
                        case 8:
                            State8();
                            break;
                        case 9:
                            State9();
                            break;
                        case 10:
                            State10();
                            break;
                        case 11:
                            State11();
                            break;
                        case 12:
                            State12();
                            break;
                        case 13:
                            State13();
                            break;
                        case 14:
                            State14();
                            break;
                        case 15:
                            State15();
                            break;
                        case 16:
                            State16();
                            break;
                        case 17:
                            State17();
                            break;
                        case 18:
                            State18();
                            break;
                        case 19:
                            State19();
                            break;
                        case 20:
                            State20();
                            break;
                        case 21:
                            State21();
                            break;
                        case 22:
                            State22();
                            break;
                        case 23:
                            State23();
                            break;
                        case 24:
                            State24();
                            break;
                        case 25:
                            State25();
                            break;
                        case 26:
                            State26();
                            break;
                        case 27:
                            State27();
                            break;
                        case 28:
                            State28();
                            break;
                        case 29:
                            State29();
                            break;
                        case 30:
                            State30();
                            break;
                        case 31:
                            State31();
                            break;
                        case 32:
                            State32();
                            break;
                        case 33:
                            State33();
                            break;
                        case 34:
                            State34();
                            break;
                        case 35:
                            State35();
                            break;
                        case 36:
                            State36();
                            break;
                        case 37:
                            State37();
                            break;
                        case 38:
                            State38();
                            break;
                        case 39:
                            State39();
                            break;
                        case 40:
                            State40();
                            break;
                        case 41:
                            State41();
                            break;
                        case 42:
                            State42();
                            break;
                        case 43:
                            State43();
                            break;
                        case 44:
                            State44();
                            break;
                    }
                }
                Succes = true;
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                MessageBox.Show($"Errror! {ex.Message}");
            }
        }

    }
}
