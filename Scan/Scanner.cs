using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using LoxNet.Tokens;
 
namespace LoxNet.Scan
{
  public class Scanner
  {
    private readonly string source;
    private readonly List<Token> tokens = new List<Token>();

    private int start = 0;
    private int current = 0;
    private int line = 1;

    public Scanner(string source)
    {
      this.source = source;
    }

    public List<Token> scanTokens()
    {
      while (!isAtEnd())
      {
        // We are at the beginning of the next lexeme.
        start = current;
        scanToken();
      }

      tokens.Add(new Token(TokenType.EOF, "", null, line));
      return tokens;
    }

    private void scanToken()
    {
      char c = advance();
      switch (c)
      {
        case '(':
          addToken(TokenType.LEFT_PAREN);
          break;
        case ')':
          addToken(TokenType.RIGHT_PAREN);
          break;
        case '{':
          addToken(TokenType.LEFT_BRACE);
          break;
        case '}':
          addToken(TokenType.RIGHT_BRACE);
          break;
        case ',':
          addToken(TokenType.COMMA);
          break;
        case '.':
          addToken(TokenType.DOT);
          break;
        case '-':
          addToken(TokenType.MINUS);
          break;
        case '+':
          addToken(TokenType.PLUS);
          break;
        case ';':
          addToken(TokenType.SEMICOLON);
          break;
        case '*':
          addToken(TokenType.STAR);
          break;
        default:
          Program.error (line, "Unexpected character.");
        break;
      }

    }

    private char advance()
    {
      current++;
      return source[current - 1];
    }

    private void addToken(TokenType type)
    {
      addToken(type, null);
    }

    private void addToken(TokenType type, Object literal)
    {
      string text = source.Slice(start, current); //****************error!!!!!
      tokens.Add(new Token(type, text, literal, line));
    }

    private bool isAtEnd()
    {
      return current >= source.Length;
    }

 
  }
}