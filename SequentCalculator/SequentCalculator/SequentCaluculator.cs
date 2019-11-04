using System;
using System.Collections.Generic;
using Elements;
using FormulaDetector;

namespace SequentCalculator
{
    /// <summary>
    /// シークエント計算用クラス
    /// </summary>
    public static class SequentCaluculator
    {
        /// <summary>
        /// 仮定
        /// </summary>
        /// <param name="p">命題</param>
        /// <param name="g">命題</param>
        /// <returns>シークエント</returns>
        public static Sequent UseAssuming(CharSequence g, CharSequence p)
        {
            return new Sequent(new List<CharSequence> { g, p }, p);
        }

        /// <summary>
        /// 増
        /// </summary>
        /// <param name="s">シークエント</param>
        /// <param name="p">命題</param>
        /// <returns></returns>
        public static Sequent UseAdding(Sequent s, CharSequence p)
        {
            s.Assumptions.Add(p);
            return s;
        }

        /// <summary>
        /// 減
        /// </summary>
        /// <param name="s">シークエント</param>
        /// <returns></returns>
        public static Sequent UseRemoving(Sequent s)
        {
            var deleteTargets = new List<int>();
            for (var i = 0; i < s.Assumptions.Count; i++)
            {
                for (var j = i + 1; j < s.Assumptions.Count; j++)
                {
                    if (s.Assumptions[i].Equals(s.Assumptions[j]))
                        if (!deleteTargets.Contains(j))
                            deleteTargets.Add(j);
                }
            }

            deleteTargets.Sort();
            deleteTargets.Reverse();

            foreach (var deleteTarget in deleteTargets)
            {
                s.Assumptions.RemoveAt(deleteTarget);
            }

            return s;
        }

        /// <summary>
        /// 減
        /// </summary>
        /// <param name="s">シークエント</param>
        /// <param name="i">交換インデックス</param>
        /// <param name="j">交換インデックス</param>
        /// <returns></returns>
        public static Sequent UseExchanging(Sequent s, int i, int j)
        {
            if (i > s.Assumptions.Count)
                throw new ArgumentException("不正なインデックスが指定されました");

            if (j > s.Assumptions.Count)
                throw new ArgumentException("不正なインデックスが指定されました");

            var iSeq = s.Assumptions[i].Copy();
            var jSeq = s.Assumptions[j].Copy();

            s.Assumptions.RemoveAt(i);
            s.Assumptions.Insert(i, jSeq);

            s.Assumptions.RemoveAt(j);
            s.Assumptions.Insert(j, iSeq);

            return s;
        }

        /// <summary>
        /// 切断
        /// </summary>
        /// <param name="s">シークエント</param>
        /// <param name="t">シークエント(切断対象)</param>
        /// <returns></returns>
        public static Sequent UseCutting(Sequent s, Sequent t)
        {
            var cuttingTargets = new List<int>();
            for (int i = 0; i < t.Assumptions.Count; i++)
            {
                if (t.Conclusion.Equals(s.Assumptions[i]))
                    cuttingTargets.Add(i);
            }

            if (cuttingTargets.Count == 0)
                return t;

            cuttingTargets.Reverse();

            foreach (var j in cuttingTargets)
            {
                t.Assumptions.RemoveAt(j);
            }

            return t;
        }

        /// <summary>
        /// 代入
        /// </summary>
        /// <param name="s"></param>
        /// <param name="x"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Sequent UseSubstitution(Sequent s, Charactor x, CharSequence t)
        {
            foreach (var seq in s.Assumptions)
            {
                seq.StashVariable(t, x);
            }

            s.Conclusion.StashVariable(t, x);

            return s;
        }
    }
}
