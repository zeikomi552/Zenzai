using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenzai.Models.Zenzai
{
    public class Persona : BindableBase
    {
        #region Ollamaでの発信側のロール
        /// <summary>
        /// Ollamaでの発信側のロール
        /// </summary>
        string _Role = "system";
        /// <summary>
        /// Ollamaでの発信側のロール
        /// </summary>
        public string Role
        {
            get
            {
                return _Role;
            }
            set
            {
                if (_Role == null || !_Role.Equals(value))
                {
                    _Role = value;
                    RaisePropertyChanged("Role");
                }
            }
        }
        #endregion

        #region ペルソナ名
        /// <summary>
        /// ペルソナ名
        /// </summary>
        string _PersonaName = string.Empty;
        /// <summary>
        /// ペルソナ名
        /// </summary>
        public string PersonaName
        {
            get
            {
                return _PersonaName;
            }
            set
            {
                if (_PersonaName == null || !_PersonaName.Equals(value))
                {
                    _PersonaName = value;
                    RaisePropertyChanged("PersonaName");
                }
            }
        }
        #endregion

        #region ペルソナ詳細(SystemMessage)
        /// <summary>
        /// ペルソナ詳細(SystemMessage)
        /// </summary>
        string _PersonaDetail = string.Empty;
        /// <summary>
        /// ペルソナ詳細(SystemMessage)
        /// </summary>
        public string PersonaDetail
        {
            get
            {
                return _PersonaDetail;
            }
            set
            {
                if (_PersonaDetail == null || !_PersonaDetail.Equals(value))
                {
                    _PersonaDetail = value;
                    RaisePropertyChanged("PersonaDetail");
                }
            }
        }
        #endregion
    }
}
