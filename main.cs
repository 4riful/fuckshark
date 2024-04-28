using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;



namespace compiler2024
{
    public partial class CompilerBeta : System.Windows.Forms.Form
    {
        public string sourceProgram;
        public int currentPointer;
        public token currentToken;
        private void print(string s)
        {
            listBox.Items.Add(s);
        }
        private void buttonTokenize_Click(object sender, EventArgs e)
        {
            sourceProgram = textBox1.Text + "#";
            listBox.Items.Clear();
            listBox1.Items.Clear();
            print(sourceProgram);
            currentPointer = 0;// Get the first token
            currentToken = tokenizer();// Print the first token
            listBox.Items.Add(currentToken.type + ", " + currentToken.value);
            // Loop to process all tokens until the end-of-input marker
            while (currentToken.type != "#")
            {
                currentToken = tokenizer();  // Get the next token
                listBox.Items.Add(currentToken.type + ", " + currentToken.value);  // Print each token
            }
            listBox.Items.Add("end..");// Indicate the end of tokenization
        }

        public CompilerBeta()
        {
            InitializeComponent();
        }
        public class token
        {
            public string type { get; set; }
            public string value { get; set; }
            public token(string t, string v)
            {
                this.type = t;
                this.value = v;
            }

            public override string ToString()
            {
                return string.Concat(new string[]
                {
                    "(",
                    this.type,
                    ",",
                    this.value,
                    ")"
                });
            }

        }
        private token tokenizer()
        {
            int state = 0;
            string word = "";

            while (sourceProgram[currentPointer] != '#')
            {
                // Skip spaces
                if (sourceProgram[currentPointer] == ' ')
                {
                    currentPointer++;
                    continue;
                }

                if (state == 0)
                {
                    if (sourceProgram[currentPointer] == ';')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        return new token("semiColon", word);
                    }

                    if (sourceProgram[currentPointer] == 'i')
                    {
                        state = 200;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] == ',')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        return new token("comma", word);
                    }
                    if (sourceProgram[currentPointer] == '$')
                    {
                        state = 400;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] == '=')
                    {
                        state = 500;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] == '(')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        return new token("leftP", word);
                    }
                    if (sourceProgram[currentPointer] == ')')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        return new token("rightP", word);
                    }
                    if (sourceProgram[currentPointer] == 't')
                    {
                        state = 800;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] == 'e')
                    {
                        state = 900;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] == 'w')
                    {
                        state = 1000;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] == 'd')
                    {
                        state = 1100;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }

                    if (sourceProgram[currentPointer] == '+')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        return new token("opPlus", word);
                    }

                    if (sourceProgram[currentPointer] == '*')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        return new token("opTime", word);
                    }
                    if (sourceProgram[currentPointer] == '<')
                    {
                        state = 1400;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] == '>')
                    {
                        state = 1500;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] == '!')
                    {
                        state = 1600;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] == 'b')
                    {
                        state = 1700;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] >= '0' && sourceProgram[currentPointer] <= '9')
                    {
                        state = 1800;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    else
                    {
                        // Handling unexpected characters
                        print("Error(@" + currentPointer.ToString() + "): Unexpected character '" + sourceProgram[currentPointer].ToString() + "'");
                        currentPointer++; // Skip the unexpected character and continue
                    }


                }

                if (state == 200)
                {
                    if (sourceProgram[currentPointer] == 'n')
                    {
                        state = 201;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] == 'f')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        state = 0;  // Reset state to 0 after creating token
                        return new token("kw_if", word);
                    }
                    else
                    {
                        // Error handling for unexpected characters in state 200
                        print("Error(@" + currentPointer.ToString() + "): Unexpected character '" + sourceProgram[currentPointer].ToString() + "' in state 200");
                        currentPointer++; // Skip the unexpected character
                        state = 0; // Reset state to start fresh
                        continue;
                    }
                }

                if (state == 201)
                {
                    if (sourceProgram[currentPointer] == 't')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        state = 0;  // Reset state to 0 after creating token
                        return new token("kw_int", word);
                    }
                    else
                    {
                        // Error handling for unexpected characters in state 200
                        print("Error(@" + currentPointer.ToString() + "): Unexpected character '" + sourceProgram[currentPointer].ToString() + "' in state 200");
                        currentPointer++; // Skip the unexpected character
                        state = 0; // Reset state to start fresh
                        continue;
                    }
                }

                if (state == 400)
                {
                    if (sourceProgram[currentPointer] >= 'a' && sourceProgram[currentPointer] <= 'z')
                    {
                        word = word + sourceProgram[currentPointer];
                        state = 401;
                        currentPointer++;
                        continue;
                    }
                    else
                    {
                        // Error handling for unexpected characters in state 200
                        print("Error(@" + currentPointer.ToString() + "): Unexpected character '" + sourceProgram[currentPointer].ToString() + "' in state 200");
                        currentPointer++; // Skip the unexpected character
                        state = 0; // Reset state to start fresh
                        continue;
                    }
                }



                if (state == 401)
                {
                    if ((sourceProgram[currentPointer] >= 'a' && sourceProgram[currentPointer] <= 'z') ||
                        (sourceProgram[currentPointer] >= '0' && sourceProgram[currentPointer] <= '9'))
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    else
                    {
                        state = 0;  // Reset state to 0 after creating token
                        return new token("identifier", word);  // Ensure consistent naming
                    }

                }



                if (state == 500)
                {
                    if (sourceProgram[currentPointer] == '=')
                    {
                        state = 501;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    state = 0;  // Reset state after handling token
                    return new token("assign", word);
                }

                if (state == 501)
                {
                    state = 0;  // Reset state after handling token
                    return new token("log_op", word);
                }
                if (state == 800)
                {
                    // Example of handling state 800, possibly for multi-letter tokens starting with 't'
                    if (sourceProgram[currentPointer] == 'h')
                    {
                        state = 801;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                }
                if (state == 801)
                {
                    if (sourceProgram[currentPointer] == 'e')
                    {
                        state = 802;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                }
                if (state == 802)
                {
                    if (sourceProgram[currentPointer] == 'n')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        state = 0;  // Reset state after completing the keyword
                        return new token("kw_then", word);
                    }
                }
                if (state == 900)
                {
                    if (sourceProgram[currentPointer] == 'l')
                    {
                        state = 901;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    if (sourceProgram[currentPointer] == 'n')
                    {
                        state = 910;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                }
                if (state == 901)
                {
                    if (sourceProgram[currentPointer] == 's')
                    {
                        state = 902;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                }
                if (state == 902)
                {
                    if (sourceProgram[currentPointer] == 'e')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        state = 0;  // Reset state after completing the keyword
                        return new token("kw_else", word);
                    }
                }
                if (state == 910)
                {
                    if (sourceProgram[currentPointer] == 'd')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        state = 0;  // Reset state after completing the keyword
                        return new token("kw_end", word);
                    }
                }
                // Handling of state 1000, potentially for keywords starting with 'w'
                if (state == 1000)
                {
                    if (sourceProgram[currentPointer] == 'h')
                    {
                        state = 1001;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                }
                if (state == 1001)
                {
                    if (sourceProgram[currentPointer] == 'i')
                    {
                        state = 1002;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                }
                if (state == 1002)
                {
                    if (sourceProgram[currentPointer] == 'l')
                    {
                        state = 1003;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                }
                if (state == 1003)
                {
                    if (sourceProgram[currentPointer] == 'e')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        state = 0;  // Reset state after completing the keyword
                        return new token("kw_while", word);
                    }
                }

                // Handling of state 1100, potentially for keywords starting with 'd'
                if (state == 1100)
                {
                    if (sourceProgram[currentPointer] == 'o')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        state = 0;  // Reset state after completing the keyword
                        return new token("kw_do", word);
                    }
                }


                // Example for states 1400 and 1500 for relational operators
                if (state == 1400)
                {
                    if (sourceProgram[currentPointer] == '=')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        state = 0;  // Reset state after forming relational operator '<='
                        return new token("log_op", word);  // 'less than or equal to'
                    }
                    else
                    {
                        state = 0;  // Reset state for a standalone '<'
                        return new token("log_op", word);  // 'less than'
                    }
                }

                if (state == 1500)
                {
                    if (sourceProgram[currentPointer] == '=')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        state = 0;  // Reset state after forming relational operator '>='
                        return new token("log_op", word);  // 'greater than or equal to'
                    }
                    else
                    {
                        state = 0;  // Reset state for a standalone '>'
                        return new token("log_op", word);  // 'greater than'
                    }
                }
                if (state == 1600)
                {
                    if (sourceProgram[currentPointer] == '=')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        state = 0;  // Reset state after forming logical operator '!='
                        return new token("log_op", word);  // 'not equal to'
                    }
                    else
                    {
                        state = 0;  // Reset state if just '!'
                        return new token("log_op", word);  // 'logical not'
                    }
                }

                // Handling of state 1700, potentially for keywords starting with 'b'
                if (state == 1700)
                {
                    if (sourceProgram[currentPointer] == 'e')
                    {
                        state = 1701;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                }
                if (state == 1701)
                {
                    if (sourceProgram[currentPointer] == 'g')
                    {
                        state = 1702;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                }
                if (state == 1702)
                {
                    if (sourceProgram[currentPointer] == 'i')
                    {
                        state = 1703;
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                }
                if (state == 1703)
                {
                    if (sourceProgram[currentPointer] == 'n')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        state = 0;  // Reset state after completing the keyword
                        return new token("kw_begin", word);
                    }
                }

                // Handling numeric literals starting with digits (state 1800)



                if (state == 1800)
                {
                    if (sourceProgram[currentPointer] >= '0' && sourceProgram[currentPointer] <= '9')
                    {
                        word += sourceProgram[currentPointer];
                        currentPointer++;
                        continue;
                    }
                    else
                    {
                        state = 0;  // Reset state after completing the number
                        return new token("integer", word);
                    }
                }
                // Reset the pointer increment and handling at the end of the while loop
                currentPointer++;
            }

            return new token("#", "#");  // Return termination token when end of input is reached
        }



        private void debug(string s)
        {
            listBox1.Items.Add(s);
        }


        private void HandleParseClick(object sender, EventArgs e)
        {
            sourceProgram = textBox1.Text + "#";
            listBox.Items.Clear();
            listBox1.Items.Clear();
            debug(sourceProgram);
            currentPointer = 0;
            currentToken = tokenizer();
            print(currentToken.ToString());
            bool flag = parseProgram();
            if (flag)
            {
                print("end..");
            }
            else
            {
                print("error...");
            }
        }
        public bool match(string expectedType)
        {
            bool flag = currentToken.type == expectedType;
            bool result;
            if (flag)
            {
                debug(string.Concat(new string[]
                {
                    "expec ",
                    expectedType,
                    ", currentToken ",
                    currentToken.type,
                    ", matched."
                }));
                currentToken = tokenizer();
                print(currentToken.ToString());
                result = true;
            }
            else
            {
                print(string.Concat(new string[]
                {
                    "Error: expec ",
                    expectedType,
                    ", got ",
                    currentToken.ToString(),
                    "."
                }));
                result = false;
            }
            return result;
        }

      
        public bool parseProgram()
        {
            print("< program > → <variable declaration section> ; <statements section>");
            bool flag = !parseDeclarationSection();
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !match("semiColon");
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = !parseStatementsSection();
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        debug("[< program > → <variable declaration section> ; <statements section>]end");
                        result = true;
                    }
                }
            }
            return result;
        }

        
        public bool parseDeclarationSection()
        {
            print("< variable declaration section > → int <variable list>");
            bool flag = !match("kw_int");
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !parseVariableList();
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    debug("[< variable declaration section > → int <variable list>]end");
                    result = true;
                }
            }
            return result;
        }

       
        public bool parseVariableList()
        {
            print("< variable list > → identifier A");
            bool flag = !match("identifier");
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !parseA();
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    debug("[< variable list > → identifier A]end");
                    result = true;
                }
            }
            return result;
        }

        
        public bool parseA()
        {
            bool flag = currentToken.type == "comma";
            bool result;
            if (flag)
            {
                print("A → , identifier A");
                bool flag2 = !match("comma");
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = !match("identifier");
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag4 = !parseA();
                        if (flag4)
                        {
                            result = false;
                        }
                        else
                        {
                            debug("[A → , identifier A]end");
                            result = true;
                        }
                    }
                }
            }
            else
            {
                bool flag5 = currentToken.type == "semiColon";
                if (flag5)
                {
                    print("A →ε");
                    result = true;
                }
                else
                {
                    print("can't choose production for parseA with " + currentToken.ToString());
                    result = false;
                }
            }
            return result;
        }

        
        public bool parseStatementsSection()
        {
            print("< statements section > → <statement>; B");
            bool flag = !parseStatement();
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !match("semiColon");
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = !parseB();
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        debug("[< statements section > → <statement>; B]end");
                        result = true;
                    }
                }
            }
            return result;
        }

   
        public bool parseB()
        {
            bool flag = currentToken.type == "identifier" || currentToken.type == "kw_if" || currentToken.type == "kw_while";
            bool result;
            if (flag)
            {
                print("B → <statement>; B");
                bool flag2 = !parseStatement();
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = !match("semiColon");
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag4 = !parseB();
                        if (flag4)
                        {
                            result = false;
                        }
                        else
                        {
                            debug("[B → <statement>; B]end");
                            result = true;
                        }
                    }
                }
            }
            else
            {
                bool flag5 = currentToken.type == "#" || currentToken.type == "kw_end";
                if (flag5)
                {
                    print("B →ε");
                    result = true;
                }
                else
                {
                    print("can't choose production for parseB with " + currentToken.ToString());
                    result = false;
                }
            }
            return result;
        }

        public bool parseStatement()
        {
            bool flag = currentToken.type == "identifier";
            bool result;
            if (flag)
            {
                print("< statement > → <assignment statement>");
                bool flag2 = !parseAssignmentStatement();
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    debug("[< statement > → <assignment statement>]end");
                    result = true;
                }
            }
            else
            {
                bool flag3 = currentToken.type == "kw_if";
                if (flag3)
                {
                    print("< statement > →<conditional statement>");
                    bool flag4 = !parseConditionalStatement();
                    if (flag4)
                    {
                        result = false;
                    }
                    else
                    {
                        debug("[< statement > →<conditional statement>]end");
                        result = true;
                    }
                }
                else
                {
                    bool flag5 = currentToken.type == "kw_while";
                    if (flag5)
                    {
                        print("< statement > →<iteration statement>");
                        bool flag6 = !parseIterationStatement();
                        if (flag6)
                        {
                            result = false;
                        }
                        else
                        {
                            debug("[< statement > →<iteration statement>]end");
                            result = true;
                        }
                    }
                    else
                    {
                        print("can't choose production for parseStatement with " + currentToken.ToString());
                        result = false;
                    }
                }
            }
            return result;
        }

        public bool parseAssignmentStatement()
        {
            print("< assignment statement > → identifier = <expression>");
            bool flag = !match("identifier");
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !match("assign");
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = !parseExpression();
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        debug("[< assignment statement > → identifier = <expression>]end");
                        result = true;
                    }
                }
            }
            return result;
        }

        public bool parseExpression()
        {
            print("< expression > →  < item > C");
            bool flag = !parseItem();
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !parseC();
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    debug("[< expression > →  < item > C]end");
                    result = true;
                }
            }
            return result;
        }

        public bool parseItem()
        {
            print("< item > →  <factor> D");
            bool flag = !parseFactor();
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !parseD();
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    debug("[< item > →  <factor> D]end");
                    result = true;
                }
            }
            return result;
        }

        public bool parseC()
        {
            bool flag = currentToken.type == "opPlus";
            bool result;
            if (flag)
            {
                print("C → + <item> C");
                bool flag2 = !match("opPlus");
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = !parseItem();
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag4 = !parseC();
                        if (flag4)
                        {
                            result = false;
                        }
                        else
                        {
                            debug("[C → + <item> C]end");
                            result = true;
                        }
                    }
                }
            }
            else
            {
                bool flag5 = currentToken.type == "semiColon" || currentToken.type == "rightP" || currentToken.type == "log_op";
                if (flag5)
                {
                    print("C →ε");
                    result = true;
                }
                else
                {
                    print("can't choose production for parseC with " + currentToken.ToString());
                    result = false;
                }
            }
            return result;
        }

      
        public bool parseFactor()
        {
            bool flag = currentToken.type == "identifier";
            bool result;
            if (flag)
            {
                print("< factor > → identifier ");
                bool flag2 = !match("identifier");
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    debug("[< factor > → identifier]end");
                    result = true;
                }
            }
            else
            {
                bool flag3 = currentToken.type == "integer";
                if (flag3)
                {
                    print("< factor > → integer ");
                    bool flag4 = !match("integer");
                    if (flag4)
                    {
                        result = false;
                    }
                    else
                    {
                        debug("[< factor > → integer]end");
                        result = true;
                    }
                }
                else
                {
                    bool flag5 = currentToken.type == "leftP";
                    if (flag5)
                    {
                        print("< factor > → (< expression >) ");
                        bool flag6 = !match("leftP");
                        if (flag6)
                        {
                            result = false;
                        }
                        else
                        {
                            bool flag7 = !parseExpression();
                            if (flag7)
                            {
                                result = false;
                            }
                            else
                            {
                                bool flag8 = !match("rightP");
                                if (flag8)
                                {
                                    result = false;
                                }
                                else
                                {
                                    debug("[< factor > → (< expression >)]end");
                                    result = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        print("can't choose production for parseFactor with " + currentToken.ToString());
                        result = false;
                    }
                }
            }
            return result;
        }

       
        public bool parseD()
        {
            bool flag = currentToken.type == "opTime";
            bool result;
            if (flag)
            {
                print("D → * < factor > D");
                bool flag2 = !match("opTime");
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = !parseFactor();
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag4 = !parseD();
                        if (flag4)
                        {
                            result = false;
                        }
                        else
                        {
                            debug("[D → * < factor > D]end");
                            result = true;
                        }
                    }
                }
            }
            else
            {
                bool flag5 = currentToken.type == "semiColon" || currentToken.type == "rightP" || currentToken.type == "log_op" || currentToken.type == "opPlus";
                if (flag5)
                {
                    print("D →ε");
                    result = true;
                }
                else
                {
                    print("can't choose production for parseD with " + currentToken.ToString());
                    result = false;
                }
            }
            return result;
        }

        public bool parseConditionalStatement()
        {
            print("< conditional statement > → if （< condition >） then <nested statement> ; else < nested statement > ");
            bool flag = !match("kw_if");
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !match("leftP");
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = !parseCondition();
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag4 = !match("rightP");
                        if (flag4)
                        {
                            result = false;
                        }
                        else
                        {
                            bool flag5 = !match("kw_then");
                            if (flag5)
                            {
                                result = false;
                            }
                            else
                            {
                                bool flag6 = !parseNestedStatement();
                                if (flag6)
                                {
                                    result = false;
                                }
                                else
                                {
                                    bool flag7 = !match("semiColon");
                                    if (flag7)
                                    {
                                        result = false;
                                    }
                                    else
                                    {
                                        bool flag8 = !match("kw_else");
                                        if (flag8)
                                        {
                                            result = false;
                                        }
                                        else
                                        {
                                            bool flag9 = !parseNestedStatement();
                                            if (flag9)
                                            {
                                                result = false;
                                            }
                                            else
                                            {
                                                debug("[< conditional statement > → if （< condition >） then <nested statement> ; else < nested statement > ");
                                                result = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        
        public bool parseCondition()
        {
            print("< expression >  logical_operator < expression >");
            bool flag = !parseExpression();
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !match("log_op");
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = !parseExpression();
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        debug("[< expression >  logical_operator < expression >]end");
                        result = true;
                    }
                }
            }
            return result;
        }

        
        public bool parseNestedStatement()
        {
            bool flag = currentToken.type == "identifier" || currentToken.type == "kw_if" || currentToken.type == "kw_while";
            bool result;
            if (flag)
            {
                print("< nested statement > → <statement>");
                bool flag2 = !parseStatement();
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    debug("[< nested statement > → <statement>]end");
                    result = true;
                }
            }
            else
            {
                bool flag3 = currentToken.type == "kw_begin";
                if (flag3)
                {
                    print("< nested statement > →<compound statement>");
                    bool flag4 = !parseCompoundStatement();
                    if (flag4)
                    {
                        result = false;
                    }
                    else
                    {
                        debug("[< nested statement > →<compound statement>]end");
                        result = true;
                    }
                }
                else
                {
                    print("can't choose production for < nested statement > with " + currentToken.ToString());
                    result = false;
                }
            }
            return result;
        }

       
        public bool parseCompoundStatement()
        {
            print("< compound statement > → begin < statements section > end");
            bool flag = !match("kw_begin");
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !parseStatementsSection();
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = !match("kw_end");
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        debug("[< compound statement > → begin < statements section > end]end");
                        result = true;
                    }
                }
            }
            return result;
        }

       
        public bool parseIterationStatement()
        {
            print("while （< condition >） do < nested statement >");
            bool flag = !match("kw_while");
            bool result;
            if (flag)
            {
                result = false;
            }
            else
            {
                bool flag2 = !match("leftP");
                if (flag2)
                {
                    result = false;
                }
                else
                {
                    bool flag3 = !parseCondition();
                    if (flag3)
                    {
                        result = false;
                    }
                    else
                    {
                        bool flag4 = !match("rightP");
                        if (flag4)
                        {
                            result = false;
                        }
                        else
                        {
                            bool flag5 = !match("kw_do");
                            if (flag5)
                            {
                                result = false;
                            }
                            else
                            {
                                bool flag6 = !parseNestedStatement();
                                if (flag6)
                                {
                                    result = false;
                                }
                                else
                                {
                                    debug("[while （< condition >） do < nested statement >]end");
                                    result = true;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
