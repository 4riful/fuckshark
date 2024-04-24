private token tokenizer()
{
    int state = 0;
    string word = "";

    while (sourceProgram[currentPointer] != '#')
    {
        // Check if the current character is a space
        if (sourceProgram[currentPointer] == ' ')
        {
            // Skip the space and continue with the next iteration of the loop
            currentPointer++;
            continue;
        }
        if (state == 0)
        {

            if (sourceProgram[currentPointer] == ';')
            {
                word = word + sourceProgram[currentPointer];
                state = 0;
                currentPointer++;
                return new token("semiColon", word);
            }

            if (sourceProgram[currentPointer] == 'i')
            {
                state = 200;
                word = word + sourceProgram[currentPointer];
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
                word = word + sourceProgram[currentPointer];
                state = 400;
                currentPointer++;
                continue;
            }

            if (this.sourceProgram[currentPointer] == '=')
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
                return new token("leftParenthesis", word);
            }
            if (sourceProgram[currentPointer] == ')')
            {
                word += sourceProgram[currentPointer];
                currentPointer++;
                return new token("rightParenthesis", word);
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
                return new token("oPTime", word);
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

            //Added Symbl According to the DFA

        }




        if (state == 200)
        {
            if (sourceProgram[currentPointer] == 'n')
            {
                state = 201;
                word = word + sourceProgram[currentPointer];
                currentPointer++;
                continue;
            }
            if (sourceProgram[currentPointer] == 'f')
            {
                word = word + sourceProgram[currentPointer];
                state = 0;
                currentPointer++;
                return new token("kw_if", word);
            }

        }


        // if the current state is 201 and the current token symbol is "t" then output the token

        if (state == 201)
        {
            if (sourceProgram[currentPointer] == 't')
            {
                word = word + sourceProgram[currentPointer];
                state = 0;
                currentPointer++;
                return new token("kw_int", word);
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
        }

        //here issuess may occur    
        if (state == 401)
        {
            if ((sourceProgram[currentPointer] >= 'a' && sourceProgram[currentPointer] <= 'z') || (sourceProgram[currentPointer] >= '0' && sourceProgram[currentPointer] <= '9'))
            {
                state = 401;
                word = word + sourceProgram[currentPointer];
                currentPointer++;
                continue;
            }
            else
            {
                state = 0;
                return new token("Identifier", word);
            }

        }

        //here issuess may occur    
        else
        {
            if (state == 500)
            {
                if (sourceProgram[currentPointer] == '=')
                {
                    state = 501;
                    word = word + sourceProgram[currentPointer];
                    currentPointer++;
                    continue;
                }
                return new token("Identifier", word);
            }
            if (state == 501)
            {
                return new token("log_op", word);
            }
        }



        if (state == 800)
        {
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
                state = 0;
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
            if (sourceProgram[currentPointer] == 's')
            {

                word += sourceProgram[currentPointer];
                currentPointer++;
                return new token("kw_else", word);
            }
        }
        if (state == 910)
        {
            if (sourceProgram[currentPointer] == 'd')
            {

                word += sourceProgram[currentPointer];
                currentPointer++;
                return new token("kw_end", word);
            }
        }
        if (state == 1000)
        {
            if (sourceProgram[currentPointer] == 's')
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
                state = 1001;
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
                return new token("kw_while", word);
            }
        }
        if (state == 1100)
        {
            if (sourceProgram[currentPointer] == 'o')
            {
         
                word += sourceProgram[currentPointer];
                currentPointer++;
                return new token("kw_do", word);
            }
        }





        // If it's not a space, print it and then proceed with your logic
        print(sourceProgram[currentPointer].ToString());

        // Move to the next character
        currentPointer++;
    }

    // When the end-of-input marker is reached, return a placeholder token
    return new token("#", "#");
}
