# rabin-karp-algorithm
Detailed explanation of the Rabin-Karp algorithm: https://youtu.be/qQ8vS2btsxI

This algorithm uses hashing to find any instances of a pattern in piece of text in linear time. Essentially, a hash value is created for the pattern by summing  each ascii value of the character in the pattern times 26 to the power of ((pattern.Length - 1)-index). We use 26 in this case because when dealing with lowercase alphabetical characters there are 26 possible options, and raising 26 to the power of ((pattern.Length - 1)-index) ensures that the amount of false positives will be minimized. However, a Verify() function is still utilized to verify any matches found.

After finding the hash value for the pattern, we can find the hash value, using the same method, for the first pattern.Length characters in the text (which is in this case the U.S. Constitution). 

Next, we can begin processing the text. We start at index 1 since we already computed the hash for characters 0-(pattern.Length-1) in the text. For every iteration of the for loop, the current hash value for the text section is modified in order to subtact the leftmost character and add the next character in the text to the section being tested with the hash value. This can be done by first subtracting (character ascii value)*26^(n-1). Next, multiply the hash value by 10 in order to increment the exponents on 26. Finally, add the ascii value of the next character in the text to complete the shift. 

Logically, the following is true:
- For a string of all the same character, such as AAAAAAAAAAAAAA, (hash) = ((hash)-((ascii value)*(26^n-1)))*26 + ((ascii value)*(26^0))
- Now consider the string ABCDEFGHI; suppose "old hash" is ABC's hash value, and "new hash" is BCD's hash value: (new hash) = ((old hash)-((A ascii value)*(26^n-1)))*26 + ((D ascii value)*(26^0))

By iterating through the text using this hash value, instances of the pattern are able to be found in O(m + n) expected time.
