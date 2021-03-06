﻿using System;
using System.Collections.Generic;
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


    private static readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>()
    {
      {"and", TokenType.AND},
      {"class", TokenType.CLASS},
      {"else", TokenType.ELSE},
      {"false", TokenType.FALSE},
      {"for", TokenType.FOR},
      {"fun", TokenType.FUN},
      {"if", TokenType.IF},
      {"nil", TokenType.NIL},
      {"or", TokenType.OR},
      {"print", TokenType.PRINT},
      {"return", TokenType.RETURN},
      {"super", TokenType.SUPER},
      {"this", TokenType.THIS},
      {"true", TokenType.TRUE},
      {"var", TokenType.VAR},
      {"while", TokenType.WHILE}
    };


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
        case '*': addToken(TokenType.STAR); break;

        case '!':
          addToken(match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
          break;
        case '=':
          addToken(match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
          break;
        case '<':
          addToken(match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
          break;
        case '>':
          addToken(match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
          break;

        case '/':
          if (match('/'))
          {
            // A comment goes until the end of the line.
            while (peek() != '\n' && !isAtEnd())
            {
              advance();
            }
          }
          else
          {
            addToken(TokenType.SLASH);
          }
          break;
          


        case ' ':
        case '\r':
        case '\t':
          // Ignore whitespace.
          break;

        case '\n':
          line++;
          break;

        case '"':
          StringLiteral();
          break;

        default:
          if (isDigit(c))
          {
            Number();
          }
          else if (isAlpha(c))
          {
            identifier();
          }
          else
          {
            Program.error(line, "Unexpected character.");
          }

          break;
      }
    }

    private bool isAlpha(char c)
    {
      return (c >= 'a' && c <= 'z') ||
             (c >= 'A' && c <= 'Z') ||
             c == '_';
    }

    private void identifier()
    {
      while (isAlphaNumeric(peek()))
      {
        advance();
      }

      string text = source.Slice(start, current);
      TokenType type = TokenType.IDENTIFIER;
      if (keywords.TryGetValue(text, out TokenType kw_type))
      {
        type = kw_type;
      }

      addToken(type);
    }

    private bool isAlphaNumeric(char c)
    {
      return isAlpha(c) || isDigit(c);
    }

    private void Number()
    {
      while (isDigit(peek()))
      {
        advance();
      }

      // Look for a fractional part.
      if (peek() == '.' && isDigit(peekNext()))
      {
        // Consume the "."
        advance();

        while (isDigit(peek()))
        {
          advance();
        }
      }

      addToken(TokenType.NUMBER, double.Parse(source.Slice(start, current)));
    }

    private bool isDigit(char c)
    {
      return c >= '0' && c <= '9';
    }

    private char peekNext()
    {
      if (current + 1 >= source.Length)
      {
        return '\0';
      }

      return source[current + 1];
    }

    private void StringLiteral()
    {
      while (peek() != '"' && !isAtEnd())
      {
        if (peek() == '\n') line++;
        advance();
      }

      if (isAtEnd())
      {
        Program.error(line, "Unterminated string.");
        return;
      }

      // The closing ".
      advance();

      // Trim the surrounding quotes.
      string value = source.Slice(start + 1, current - 1);
      addToken(TokenType.STRING, value);
    }


    private bool match(char expected)
    {
      if (isAtEnd())
        return false;

      if (source[current] != expected)
        return false;

      current++;

      return true;
    }

    private char peek()
    {
      if (isAtEnd())
        return '\0';

      return source[current];
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
      string text = source.Slice(start, current);
      tokens.Add(new Token(type, text, literal, line));
    }

    private bool isAtEnd()
    {
      return current >= source.Length;
    }
  }
}