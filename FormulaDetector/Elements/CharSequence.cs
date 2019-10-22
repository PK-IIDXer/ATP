using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elements
{
    /// <summary>
    /// 文字列クラス
    /// </summary>
    public class CharSequence
    {
        #region enums
        /// <summary>
        /// 項型か命題型か
        /// </summary>
        public enum TermPropTypes
        {
            /// <summary>
            /// 指定なし
            /// </summary>
            None,
            /// <summary>
            /// 項型文字列
            /// </summary>
            Term,
            /// <summary>
            /// 命題型文字列
            /// </summary>
            Proposition
        }
        #endregion

        #region プロパティ
        /// <summary>
        /// 文字列本体
        /// </summary>
        public readonly Charactor[] Body = null;
        /// <summary>
        /// 鎖
        /// </summary>
        public readonly Tuple<Charactor, Charactor>[] Chains = null;
        /// <summary>
        /// 項型か命題型か
        /// </summary>
        public readonly TermPropTypes TermPropType;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 文字列インスタンスのコンストラクタです。
        /// </summary>
        /// <param name="charactors">文字列本体</param>
        /// <param name="chains">鎖</param>
        public CharSequence(Charactor[] charactors, Tuple<Charactor,Charactor>[] chains)
        {
            if (charactors == null || charactors.Length == 0)
                throw new ArgumentException("最低一文字は指定してください");

            Body = charactors;
            Chains = chains;
            TermPropType = GetTermPropType(charactors);
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 項型・命題型を取得します
        /// </summary>
        /// <param name="charactor">文字列</param>
        /// <returns>項型・命題型</returns>
        private TermPropTypes GetTermPropType(Charactor[] charactors)
        {
            Charactor charactor = charactors[0];

            if (charactor.Type == Charactor.Types.Value)
                return TermPropTypes.Term;

            if (charactor.Type == Charactor.Types.Function)
                return TermPropTypes.Term;

            if (charactor.Type == Charactor.Types.Quantifier
                && charactor.TermPropType == Charactor.TermPropTypes.Term)
                return TermPropTypes.Term;

            if (charactor.Type == Charactor.Types.Predicate)
                return TermPropTypes.Proposition;

            if (charactor.Type == Charactor.Types.Conjunction)
                return TermPropTypes.Proposition;

            if (charactor.Type == Charactor.Types.Quantifier
                && charactor.TermPropType == Charactor.TermPropTypes.Proposition)
                return TermPropTypes.Proposition;

            return TermPropTypes.None;
        }

        public override string ToString()
        {
            string result = "";
            foreach (var body in Body)
            {
                result += body.ToString();
            }
            return result;
        }
        #endregion
    }
}
