using System;
using System.Collections.Generic;
using System.Text;

namespace LoxNet.Tokens
{
  public class Token
  {
     public TokenType type { get; }
     public string lexeme { get; }
     public object literal { get; }
     public int line { get; }


    public Token(TokenType type, string lexeme, object literal, int line)
    {
      this.type = type;
      this.lexeme = lexeme;
      this.literal = literal;
      this.line = line;
    }

    public string toString()
    {
      return type + " " + lexeme + " " + literal;
    }
  }
}
