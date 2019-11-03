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
        public readonly List<Charactor> Body = null;
        /// <summary>
        /// 鎖
        /// </summary>
        public readonly List<Tuple<Charactor, Charactor>> Chains = null;
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
        public CharSequence(List<Charactor> charactors, List<Tuple<Charactor,Charactor>> chains)
        {
            if (charactors == null || charactors.Count == 0)
                throw new ArgumentException("最低一文字は指定してください");

            Body = charactors;
            Chains = chains;
            TermPropType = GetTermPropType(charactors);
        }

        /// <summary>
        /// 量化用コンストラクタ
        /// </summary>
        /// <param name="quantifier">量化記号</param>
        /// <param name="value">変数</param>
        /// <param name="sequence">文字列</param>
        public CharSequence(Charactor quantifier, Charactor value, CharSequence sequence)
        {
            if (quantifier.Type != Charactor.Types.Quantifier)
                throw new ArgumentException("量化記号を指定してください");

            if (value.Type != Charactor.Types.Value)
                throw new ArgumentException("変数型を指定してください");

            var bodyList = new List<Charactor> { quantifier };
            bodyList.AddRange(sequence.Body);

            // 量化変数を□に置き換え
            var chains = new List<Tuple<Charactor, Charactor>>();
            var body = bodyList.Select(e =>
            {
                if (e.Name == value.Name)
                {
                    var sqr = new Charactor(e.Name, Charactor.Types.Square, 0, e.TermPropType);
                    return sqr;
                }

                return e;
            }).ToList();

            // 量化記号と□をchainで結ぶ
            foreach(var chr in body)
            {
                if (chr.Name == value.Name)
                    chains.Add(new Tuple<Charactor, Charactor>(quantifier, chr));
            }

            Body = body;
            Chains = chains;
            TermPropType = GetTermPropType(bodyList);
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 項型・命題型を取得します
        /// </summary>
        /// <param name="charactor">文字列</param>
        /// <returns>項型・命題型</returns>
        private TermPropTypes GetTermPropType(List<Charactor> charactors)
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

        /// <summary>
        /// 退避変換
        /// </summary>
        /// <param name="t">代入する文字列</param>
        /// <param name="x">代入される変数</param>
        public void StashVariable(CharSequence t, Charactor x)
        {
            if (x.Type != Charactor.Types.Value)
                throw new ArgumentException(string.Format("{0}には、変数型文字を指定してください", nameof(x)));

            if (t.TermPropType != TermPropTypes.Term)
                throw new ArgumentException(string.Format("{0}には、命題型文字列を指定してください。", nameof(t)));

            foreach (var chr in Body)
            {
                foreach (var tg in t.Body)
                {
                    if (chr.Name == tg.Name)
                        throw new ArgumentException(string.Format("変換対象文字列に含まれる文字が重複しています"));
                }
            }

            var tLength = t.Body.Count;
            var bLength = Body.Count;

            var counter = 0;
            for (var i = 0; i < bLength; i++)
            {
                if (Body[i + counter * (tLength - 1)].Name == x.Name
                    && Body[i + counter * (tLength - 1)].Type == x.Type)
                {
                    Body.RemoveAt(i + counter * (tLength - 1));
                    Body.InsertRange(i + counter * (tLength - 1), t.Body);

                    counter++;
                }
            }
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
