using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab1TAu
{
    public class AnalyseTokens
    {
        List<Token> tokens;
        public bool Succes = false;
        int i;
        public AnalyseTokens(List<Token> tokens) 
        {
            this.tokens = tokens;
        }
        public void Start()
        {
            i = 0;
            try
            {
                Program();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                MessageBox.Show($"Errror! {ex.Message}");
            }
        }
        public void Next()
        {
            if (i<tokens.Count-1)
            {
            i++;
            }
        }
        public void Error(Token.TokenType Ozhidal, Token.TokenType Poluch)
        {
            throw new Exception($"Ожидался один из TokenType: {Ozhidal}, а получено: {Poluch} {i}");
        }
        public void Finish()
        {

        }
        public void Expr()
        {
            while (tokens[i].Type != Token.TokenType.ENTER)
            {
                i++;
            }
        }

        public void Program()
        {
            Succes = false;
            ObList();
            OperList();
            Succes = true;
        }
        public void ObList()
        {
            if (tokens[i].Type != Token.TokenType.DIM)
                Error(Token.TokenType.DIM, tokens[i].Type);
            Next();
            if (tokens[i].Type != Token.TokenType.IDENTIFIER)
                Error(Token.TokenType.IDENTIFIER, tokens[i].Type);
            Next();
            if (tokens[i].Type != Token.TokenType.AS)
                Error(Token.TokenType.AS, tokens[i].Type);
            Next();
            Type();
            if (tokens[i].Type != Token.TokenType.ENTER)
                Error(Token.TokenType.ENTER, tokens[i].Type);
            Next();
            DopOb();
        }
        public void DopOb()
        {
            if (tokens[i].Type == Token.TokenType.DIM)
            ObList();
            if (tokens[i].Type != Token.TokenType.DIM &&
                tokens[i].Type != Token.TokenType.IDENTIFIER&&
                tokens[i].Type != Token.TokenType.IF)
                Error(Token.TokenType.DIM, tokens[i].Type); //Варианты
        }
        public void OperList()
        {
            Oper();
            if (tokens[i].Type != Token.TokenType.ENTER)
                Error(Token.TokenType.ENTER, tokens[i].Type);
            Next();
            DopOper();
        }
        public void Oper()
        {
            if (tokens[i].Type == Token.TokenType.IDENTIFIER || tokens[i].Type == Token.TokenType.IF)
            {
                if (tokens[i].Type == Token.TokenType.IDENTIFIER)
                    Prisvoen();
                if (tokens[i].Type == Token.TokenType.IF)
                    If();
            }
            else
                Error(Token.TokenType.IDENTIFIER, tokens[i].Type); //Варианты
        }
        public void DopOper()
        {
            if (tokens[i].Type != Token.TokenType.ENTER &&
                tokens[i].Type != Token.TokenType.IDENTIFIER &&
                tokens[i].Type != Token.TokenType.IF &&
                tokens[i].Type != Token.TokenType.END &&
                tokens[i].Type != Token.TokenType.ELSE)
                Error(Token.TokenType.ENTER, tokens[i].Type); // Варианты
            if (tokens[i].Type == Token.TokenType.IF || tokens[i].Type == Token.TokenType.IDENTIFIER)
                OperList();
            if (tokens[i].Type == Token.TokenType.ENTER)
            {
                Finish();
                Next();
            }
        }
        public void Type()
        {
            if (tokens[i].Type != Token.TokenType.INTEGER &&
                tokens[i].Type != Token.TokenType.BOOL &&
                tokens[i].Type != Token.TokenType.STRING)
                Error(Token.TokenType.INTEGER, tokens[i].Type); // Варианты
                Next();

        }
        public void Prisvoen()
        {
            if (tokens[i].Type != Token.TokenType.IDENTIFIER)
                Error(Token.TokenType.IDENTIFIER, tokens[i].Type);
            Next();
            if (tokens[i].Type != Token.TokenType.EQUAL)
                Error(Token.TokenType.EQUAL, tokens[i].Type);
            Next();
            Expr();
        }
        public void Sign()
        {
            if (tokens[i].Type != Token.TokenType.PLUS &&
                tokens[i].Type != Token.TokenType.MINUS &&
                tokens[i].Type != Token.TokenType.MULTIPLY &&
                tokens[i].Type != Token.TokenType.MORE &&
                tokens[i].Type != Token.TokenType.LESS &&
                tokens[i].Type != Token.TokenType.DIVISION)
                Error(Token.TokenType.PLUS, tokens[i].Type); // Варианты
            Next();
        }
        public void Operand()
        {
            if (tokens[i].Type != Token.TokenType.IDENTIFIER &&
                tokens[i].Type != Token.TokenType.LITERAL)
                Error(Token.TokenType.IDENTIFIER, tokens[i].Type); // Варианты
            Next();
        }
        public void HardOperand()
        {
            Operand();
            Sign();
            Operand();
        }
        public void If()
        {
            if (tokens[i].Type != Token.TokenType.IF)
                Error(Token.TokenType.IF, tokens[i].Type);
            Next();
            HardOperand();
            if (tokens[i].Type != Token.TokenType.THEN)
                Error(Token.TokenType.THEN, tokens[i].Type);
            Next();
            if (tokens[i].Type != Token.TokenType.ENTER)
                Error(Token.TokenType.ENTER, tokens[i].Type);
            Next();
            OperList();
            IfElse();
            if (tokens[i].Type != Token.TokenType.END)
                Error(Token.TokenType.END, tokens[i].Type);
            Next();
            if (tokens[i].Type != Token.TokenType.IF)
                Error(Token.TokenType.IF, tokens[i].Type);
            Next();
        }
        public void IfElse()
        {
            if (tokens[i].Type != Token.TokenType.ELSE &&
                tokens[i].Type != Token.TokenType.ENTER)
                Error(Token.TokenType.ELSE, tokens[i].Type); //Варианты
            if (tokens[i].Type == Token.TokenType.ELSE)
            {
                Next();
                if (tokens[i].Type != Token.TokenType.ENTER)
                    Error(Token.TokenType.ENTER, tokens[i].Type);
                Next();
                OperList();
            }
        }
    }
}
