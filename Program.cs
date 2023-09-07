if (args.Length >= 1) {
    if (args[0] == "read") {
        ReadCheeps();
    }
    if (args[0] == "cheep") {
        StoreCheep();
    }
}
else {
    Console.WriteLine("Use argument: 'read' or 'cheep'");
    return;
}

void ReadCheeps() {
    bool isDelim(char c)      => c == ',' || isNewline(c);
    bool isNewline(char c)    => c == '\n'|| c == '\r';
    bool isWhitespace(char c) => c == ' ' || c == '\t' || isNewline(c);

    string csv = File.ReadAllText("chirp_cli_db.csv") + "\n";
    //IEnumerator<(char, int)> reader = csv.Select((c, i) => {Console.WriteLine((c, i)); return (c, i);}).GetEnumerator();
    IEnumerator<(char, int)> reader = csv.Select((c, i) => (c, i)).GetEnumerator();

    List<string> messages = new List<string>();
    /* get messages */ {
        uint? authorColomn = null, messageColomn = null, timestampColomn = null;
        int startOfValue = 0;

        /* read headers */ {
            uint headerCount = 0;

            while (reader.MoveNext()) {
                (char c, int i) = reader.Current;

                if (isDelim(c)) {
                    string header = csv.Substring(startOfValue, i - startOfValue);
                    if      (authorColomn == null    && header == "Author")    authorColomn = headerCount;
                    else if (messageColomn == null   && header == "Message")   messageColomn = headerCount;
                    else if (timestampColomn == null && header == "Timestamp") timestampColomn = headerCount;

                    startOfValue = i + 1;

                    if (isNewline(c)) break;

                    headerCount++;
                }
            }

            if (authorColomn == null || messageColomn == null || timestampColomn == null) {
                Console.WriteLine("Error: CSV file needs at least the following headers: 'Author', 'Message', 'Timestamp'");
                return;
            }
        }

        /* read messages */ {
            uint lineCount = 2; // line 1 is the headers

            nextLine: {
                string? author = null, message = null, timestamp = null;
                uint valueCount = 0;
                bool isEndOfFile = false;
                bool isLineEmpty = true;

                nextValue: {
                    if (!reader.MoveNext()) {
                        isEndOfFile = true;
                        goto endOfLine;
                    }

                    var (c, i) = reader.Current;

                    while (isWhitespace(c)) { // ignore whitespace before first character
                        if (!reader.MoveNext()) {
                            isEndOfFile = true;
                            goto endOfLine;
                        }
                        (c, i) = reader.Current;
                    }
                    startOfValue = i;

                    string? value = null; // won't ever be null when reading, but compiler can't proof that.
                    bool isEndOfLine = false;

                    if (c == '"') {
                        if (i != startOfValue) {
                            Console.WriteLine($"Error: CSV file has a quote character in the middle of a value on line {lineCount}, at value {valueCount}");
                            return;
                        }

                        nextQuotedValueCharacter: {
                            if (!reader.MoveNext())
                                goto missingClosingQuote;

                            (c, i) = reader.Current;

                            if (isNewline(c))
                                goto missingClosingQuote;

                            if (c == '"') {
                                value = csv.Substring(startOfValue + 1, i - startOfValue - 1); // +/-1 to ignore quotation marks

                                skipWhitespaceAfterQuote: {
                                    if (!reader.MoveNext()) {
                                        isEndOfFile = true;
                                        goto endOfValue;
                                    }

                                    (c, i) = reader.Current;

                                    if (isDelim(c)) {
                                        if (isNewline(c)) isEndOfLine = true;
                                        goto endOfValue;
                                    }

                                    if (isWhitespace(c))
                                        goto skipWhitespaceAfterQuote;

                                    Console.WriteLine($"Error: CSV file cannot have characters in between quoted value and delimeter on line {lineCount}, at value {valueCount}");
                                    return;
                                }
                                
                            }

                            goto nextQuotedValueCharacter;

                            missingClosingQuote:
                            Console.WriteLine($"Error: CSV file is missing a closing quote on line {lineCount}, at value {valueCount}");
                            return;
                        }
                    }

                    nextValueCharacter: {
                        if (isDelim(c) || isWhitespace(c)) {
                            value = csv.Substring(startOfValue, i - startOfValue);

                            if (isNewline(c)) isEndOfLine = true;
                            
                            if (isWhitespace(c)) {
                                nextWhitespace: {
                                    if (!reader.MoveNext()) {
                                        isEndOfFile = true;
                                        goto endOfValue;
                                    }

                                    (c, i) = reader.Current;

                                    if (isNewline(c)) {
                                        isEndOfLine = true;
                                        goto endOfValue;
                                    }

                                    if (isDelim(c))
                                        goto endOfValue;
                                    
                                    if (isWhitespace(c))
                                        goto nextWhitespace;
                                }
                            }

                            goto endOfValue;
                        }

                        if (!reader.MoveNext()) {
                            isEndOfFile = true;
                            goto endOfValue;
                        }

                        (c, i) = reader.Current;

                        if (c == '"') {
                            Console.WriteLine($"Error: CSV file cannot have a quote character in the middle of a value on line {lineCount}, at value {valueCount}");
                            return;
                        }

                        goto nextValueCharacter;
                    }

                    endOfValue: {
                        isLineEmpty = false;

                        if      (valueCount == authorColomn)    author    = value;
                        else if (valueCount == messageColomn)   message   = value;
                        else if (valueCount == timestampColomn) timestamp = value;

                        if (isEndOfLine || isEndOfFile)
                            goto endOfLine;

                        valueCount++;
                        goto nextValue;
                    }
                }

                endOfLine: {
                    if (!isLineEmpty) {
                        if (author == null || message == null || timestamp == null) {
                            string missing = author == null ? "Author" : message == null ? "Message" : "Timestamp";
                            Console.WriteLine($"Error: CSV file is missing '{missing}' on line {lineCount}");
                            return;
                        }

                        if (long.TryParse(timestamp, out long time)) {
                            DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds(time).ToLocalTime();
                            messages.Add($"{author} @ {date.ToString("MM\\/dd\\/yy HH:mm:ss")}: {message}");
                        }
                        else {
                            Console.WriteLine($"Error: CSV file has an invalid timestamp on line {lineCount}");
                            return;
                        }
                    }

                    if (isEndOfFile)
                        goto endOfFile;

                    lineCount++;
                    goto nextLine;
                }
            }

            endOfFile:;
        }

        /* print messages */ {
            foreach (string message in messages) {
                Console.WriteLine(message);
            }
        }
    }
}

void StoreCheep() {
    string cheep = "";

    for (uint i = 1; i < args.Length; i++) {
        
    }
}
