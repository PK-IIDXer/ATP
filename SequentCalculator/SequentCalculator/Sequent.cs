using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elements;

namespace SequentCalculator
{
    /// <summary>
    /// シークエントクラス
    /// </summary>
    public class Sequent
    {
        /// <summary>
        /// 仮定
        /// </summary>
        public List<CharSequence> Assumptions { get; private set; }
        /// <summary>
        /// 結論
        /// </summary>
        public CharSequence Conclusion { get; private set; }

        public Sequent(List<CharSequence> assumptions, CharSequence conclusion)
        {
            foreach (var assumption in assumptions)
            {
                if (assumption.TermPropType != CharSequence.TermPropTypes.Proposition)
                    throw new ArgumentException("命題以外が入力されました。");
            }

            if (conclusion.TermPropType != CharSequence.TermPropTypes.Proposition)
                throw new ArgumentException("命題以外が入力されました。");

            Assumptions = assumptions;
            Conclusion = conclusion;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (Assumptions != null || Assumptions.Count != 0)
            {
                foreach (var assumption in Assumptions)
                {
                    sb.Append(assumption.ToString() + ",");
                }

                sb.Remove(sb.Length, 1);
                sb.Append("→");
            }

            if (Conclusion != null)
            {
                sb.Append(Conclusion.ToString());
            }
            
            return sb.ToString();
        }
    }
}
