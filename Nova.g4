grammar Nova;

// Keywords (this is not a production rule)
//KEYWORDS:-
//  PACKAGE
//  MODULE
//  LET
//  I16
    
expr
    : let
    ;

container: (module|expr);

// Constructs
package
    : PACKAGE ID LBRA (module NEWLINE*)* RBRA
    ;

module
    : MODULE ID LBRA (container NEWLINE*)* RBRA
    ;
    
let
    : LET ID EQ NEWLINE*
        (
            lambda (NEWLINE*|SEMI?) 
            |
            LBRA 
            NEWLINE* 
                (expr (NEWLINE*|SEMI?))* 
                (atom|binary)
            NEWLINE* 
            RBRA 
            | 
            (atom|binary) (NEWLINE*|SEMI?)
        )
    ;

lambda
    : LAMBDA paramList NEWLINE* ARROW NEWLINE* (literal | binary | ID) (SEMI | NEWLINE)
    | LAMBDA paramList NEWLINE* ARROW NEWLINE* 
        LBRA 
        NEWLINE* 
            (expr (NEWLINE* | SEMI?))*
            (lambda | (binary|LPAR binary RPAR) | literal | identifier)
        NEWLINE* 
        RBRA  
    ;

paramList
    : ID (ID)* 
    ;

    
literal
    : I16
    ;

binary
    : addition
    ;

addition
    : multiplication ( (PLUS | MINUS) multiplication )*
    ;

multiplication
    : atom ( (MULT | DIV) atom )*
    ;

// Atomic values (base units)
atom
    : lambda
    | LPAR binary RPAR
    | I16
    | identifier
    ;
    
identifier: ID;
    
// Keywords (reserved words)
PACKAGE : 'package';
MODULE  : 'module';
LET     : 'let';

LAMBDA  : '\\';

// Types
I16     : [0-9]+;

// Identifiers
ID      : [a-zA-Z_][a-zA-Z0-9_]*;

// Punctuation
ARROW   : '=>';
PLUS    : '+';
MINUS   : '-';
MULT    : '*';
DIV     : '/';
MOD     : '%';
LPAR    : '(';
RPAR    : ')';
COLON   : ':';
SEMI   : ';';
LBRA    : '{';
RBRA    : '}';
EQ      : '=';
LT      : '<';
GT      : '>';
LTE     : '<=';
GTE     : '>=';

// Handling newlines and skipping them
NEWLINE : '\r'? '\n' -> channel(HIDDEN);

// Handling whitespace and putting it to the hidden channel
WS      : [ \t]+ -> channel(HIDDEN);