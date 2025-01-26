using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Zenzai.Models.A1111
{
    public class PostResponseM : RequestModel
    {
        #region images[Images]プロパティ
        /// <summary>
        /// images[Images]プロパティ用変数
        /// </summary>
        List<string> _Images = new List<string>();
        /// <summary>
        /// images[Images]プロパティ
        /// </summary>
        [JsonPropertyName("images")]
        public List<string> Images
        {
            get
            {
                return _Images;
            }
            set
            {
                if (_Images == null || !_Images.Equals(value))
                {
                    _Images = value;
                    RaisePropertyChanged("Phrase");
                }
            }
        }
        #endregion
    }
}
