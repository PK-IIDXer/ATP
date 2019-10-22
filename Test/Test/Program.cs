using System;
using Elements;
using FormulaDetector;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var f = new Charactor("f", Charactor.Types.Function, 3);
            var x = new Charactor("x", Charactor.Types.Value);
            var g = new Charactor("g", Charactor.Types.Function, 2);
            var y = new Charactor("y", Charactor.Types.Value);
            var h = new Charactor("h", Charactor.Types.Function, 1);
            var z = new Charactor("z", Charactor.Types.Value);
            var w = new Charactor("w", Charactor.Types.Value);

            var s = new Charactor[7];
            s[0] = f;
            s[1] = x;
            s[2] = g;
            s[3] = y;
            s[4] = h;
            s[5] = z;
            s[6] = w;


            var str = new CharSequence(s, null);

            Console.WriteLine(FormulaValidator.Validate(str));
        }
    }
}
