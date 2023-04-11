using System;
using System.Collections.Generic;

namespace lab1TAu
{
    public class Token
    {
        public TokenType Type;
        public string Value;
        public Token(TokenType type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return string.Format("{0}, {1}", Type, Value);
        }


        public enum TokenType
        {
            AS, DIM, THEN, INTEGER, BOOL, LITERAL, IDENTIFIER, END,
            IF, ELSE, TRUE, FALSE, PLUS, MORE, LESS,
            MINUS, EQUAL, MULTIPLY, RPAR, LPAR, ENTER, DIVISION, COMMA,
            STRING
        }

        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>()
        {
            { "integer", TokenType.INTEGER },
            { "string", TokenType.STRING },
            { "bool", TokenType.BOOL },
            { "if", TokenType.IF },
            { "else", TokenType.ELSE },
            { "Dim", TokenType.DIM },
            { "end", TokenType.END },
            { "as", TokenType.AS },
            { "then", TokenType.THEN },
        };

        public static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;
            }
            return SpecialWords.ContainsKey(word);
        }
        public static Dictionary<char, TokenType> SpecialSymbols = new Dictionary<char, TokenType>()
        {
        { '#', TokenType.ENTER },
        { '(', TokenType.LPAR },
        { ')', TokenType.RPAR },
        { '+', TokenType.PLUS },
        { '-', TokenType.MINUS },
        { '=', TokenType.EQUAL },
        { '>', TokenType.MORE },
        { '<', TokenType.LESS },
        { '*', TokenType.MULTIPLY },
        { '/',  TokenType.DIVISION },
        { ',',  TokenType.COMMA },
        };

        public static bool IsSpecialSymbol(char ch)
        {
            return SpecialSymbols.ContainsKey(ch);
        }
        public static void PrintTokens(System.Windows.Forms.TextBox textbox, List<Token> list )
        {
            textbox.Text = "";
            int i = 0;
            foreach (var t in list)
            {
                textbox.Text += $"{i++}. {t} ";
                //textbox.Text += $"{t} ";
                textbox.Text += Environment.NewLine;
            }
        }
    }
}
