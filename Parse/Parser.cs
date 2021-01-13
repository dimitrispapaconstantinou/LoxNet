using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using LoxNet.AST;
using LoxNet.Tokens;
using Microsoft.VisualBasic;

/*
 *
 *
 *
expression     → equality ;
equality       → comparison ( ( "!=" | "==" ) comparison )* ;
comparison     → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
term           → factor ( ( "-" | "+" ) factor )* ;
factor         → unary ( ( "/" | "*" ) unary )* ;
unary          → ( "!" | "-" ) unary | primary ;
primary        → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" ;
 */
namespace LoxNet.Parse
{
   public  class Parser
    {
        private readonly List<Token> tokens;
        private int current = 0;

        public Parser(List<Token> tokens)
        {
            this.tokens = tokens;
        }

        //expression     → equality ;
        private Expr expression()
        {
            return equality();
        }

        //equality       → comparison ( ( "!=" | "==" ) comparison )* ;
        private Expr equality()
        {
            Expr expr = comparison();

            while ( match(TokenType.BANG, TokenType.BANG_EQUAL) )
            {
                Token op = previous();
                Expr right = comparison();
                expr = new Expr.Binary(expr,op,right);
            }

            return expr;
        }


        //comparison     → term(( ">" | ">=" | "<" | "<=" ) term )* ;
        private Expr comparison()
        {
            Expr expr = term();

            while (match(TokenType.GREATER, TokenType.GREATER_EQUAL, TokenType.LESS, TokenType.LESS_EQUAL))
            {
                Token op = previous();
                Expr right = term();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        //term           → factor(( "-" | "+" ) factor )* ;
        private Expr term()
        {
            Expr expr = factor();

            while (match(TokenType.MINUS, TokenType.PLUS))
            {
                Token op = previous();
                Expr right = factor();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        //factor         → unary(( "/" | "*" ) unary )* ;
        private Expr factor()
        {
            Expr expr = unary();

            while (match(TokenType.SLASH, TokenType.STAR))
            {
                Token op = previous();
                Expr right = unary();
                expr = new Expr.Binary(expr, op, right);
            }

            return expr;
        }

        //unary          → ( "!" | "-" ) unary | primary ;
        private Expr unary()
        {
            if (match(TokenType.BANG,TokenType.MINUS) )
            {
                Token op = previous();
                Expr right = unary();
                return new Expr.Unary(op, right);
            }
            
            return primary();
        }

        //primary        → NUMBER | STRING | "true" | "false" | "nil" | "(" expression ")" ;
        private Expr primary()
        {
            if (match(TokenType.FALSE)) 
                return new Expr.Literal(false);

            if (match(TokenType.TRUE)) 
                return new Expr.Literal(true);

            if (match(TokenType.NIL)) 
                return new Expr.Literal(null);

            if (match(TokenType.NUMBER, TokenType.STRING))
            {
                return new Expr.Literal(previous().literal);
            }

            if (match(TokenType.LEFT_PAREN))
            {
                Expr expr = expression();
                consume(TokenType.RIGHT_PAREN, "Expect ')' after expression.");
                return new Expr.Grouping(expr);
            }

            // Unknown token
            throw new Exception();
        }


        private bool match(params TokenType[] types)
        {
            foreach (TokenType type in types)
            {
                if (check(type))
                {
                    advance();
                    return true;
                }
            }

            return false;
        }

        private bool check(TokenType type)
        {
            if (isAtEnd())
            {
                return false;
            }
            return peek().type == type;
        }



        
        private Token advance()
        {
            if (!isAtEnd())
            {
                current++;
            }
            return previous();
        }

        private bool isAtEnd()
        {
            return peek().type == TokenType.EOF;
        }

        private Token peek()
        {
            return tokens[current];
        }

        private Token previous()
        {
            return tokens[current - 1];
        }

    }
}
