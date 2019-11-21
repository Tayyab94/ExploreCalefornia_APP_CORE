using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ExploreCalefornia.Models
{
    public class Post
    {
        public long Id { get; set; }

        private string _key;

        public string Key
        {
            get {
                if(_key==null)
                {
                    _key = Regex.Replace(Title.ToLower(), "[a-z0-9]", "-");
                }
                return _key;
            }
            set { _key = value; }
        }

        [Display(Name ="Post Title")]
        [Required]
        [DataType(DataType.Text)]
        [StringLength(100,ErrorMessage ="Title Should be 5 to 100 Charactors Long",MinimumLength =5)]
        public string Title { get; set; }
        public string Auther { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [StringLength(100, ErrorMessage = "Title Should be 5 to 100 Charactors Long", MinimumLength = 5)]
        public string Body { get; set; }

        public DateTime PostedDate { get; set; }
    }

}
