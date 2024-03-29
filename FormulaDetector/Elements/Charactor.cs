﻿using System;

namespace Elements
{
    /// <summary>
    /// 文字クラス
    /// </summary>
    public class Charactor
    {
        #region 性質一覧
        /// <summary>
        /// 文字の種類
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// 変数
            /// </summary>
            Value,
            /// <summary>
            /// 記号□
            /// </summary>
            Square,
            /// <summary>
            /// 関数
            /// </summary>
            Function,
            /// <summary>
            /// 述語記号
            /// </summary>
            Predicate,
            /// <summary>
            /// 論理接続詞
            /// </summary>
            Conjunction,
            /// <summary>
            /// 量化記号
            /// </summary>
            Quantifier
        }

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
            /// 項型
            /// </summary>
            Term,
            /// <summary>
            /// 命題型
            /// </summary>
            Proposition
        }
        #endregion

        #region 文字の性質
        public string Name { get; private set; }
        /// <summary>
        /// 文字の種類
        /// </summary>
        public Types Type { get; private set; }

        /// <summary>
        /// 引数の個数
        /// </summary>
        public int ArgCount { get; private set; }

        /// <summary>
        /// 項型か命題型か
        /// </summary>
        public readonly TermPropTypes TermPropType = TermPropTypes.None;
        #endregion

        #region コンストラクタ
        /// <summary>
        /// 文字を生成します。
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="type">文字のタイプ</param>
        /// <param name="argCnt">引数の個数</param>
        /// <param name="termPropType">項・命題型</param>
        public Charactor(
            string name,
            Types type,
            int argCnt = 0,
            TermPropTypes termPropType = TermPropTypes.None)
        {
            Name = name;

            if (argCnt != 0)
            {
                if (type == Types.Value)
                    throw new ArgumentException("引数の個数を指定できない文字タイプです");

                if (type == Types.Square)
                    throw new ArgumentException("引数の個数を指定できない文字タイプです");
            }

            if (termPropType != TermPropTypes.None)
            {
                if (type != Types.Quantifier && type != Types.Square)
                    throw new ArgumentException("項・命題型を指定できない文字タイプです");
            }

            if (type == Types.Quantifier)
            {
                argCnt = 1;
            }

            Type = type;
            ArgCount = argCnt;
            TermPropType = GetTermPropType(type, termPropType);
        }
        #endregion

        #region メソッド
        /// <summary>
        /// 項・命題型を取得します。
        /// </summary>
        /// <param name="type">文字</param>
        /// <param name="termPropType">項・命題型</param>
        /// <returns>項・命題型</returns>
        private TermPropTypes GetTermPropType(
            Types type,
            TermPropTypes termPropType)
        {
            switch (type)
            {
                case Types.Value:
                    return TermPropTypes.Term;
                case Types.Square:
                    return termPropType;
                case Types.Function:
                    return TermPropTypes.Term;
                case Types.Predicate:
                    return TermPropTypes.Proposition;
                case Types.Conjunction:
                    return TermPropTypes.Proposition;
                case Types.Quantifier:
                    return termPropType;
                default:
                    return TermPropTypes.None;
            }
        }

        public Charactor Copy()
        {
            return new Charactor(Name, Type, ArgCount, TermPropType);
        }

        public bool Equals(Charactor charactor)
        {
            if (Name != charactor.Name)
                return false;

            if (Type != charactor.Type)
                return false;

            if (ArgCount != charactor.ArgCount)
                return false;

            if (TermPropType != charactor.TermPropType)
                return false;

            return true;
        }

        public override string ToString()
        {
            return string.Format("[{0}]", Name);
        }
        #endregion
    }
}
