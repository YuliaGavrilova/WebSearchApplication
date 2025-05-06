using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebSearch.Shared
{
    public class Utilities
    {
        public static List<SelectListItem> GetEnumOptions<TEnum>(bool addNotSelected) where TEnum : Enum
        {
            List<SelectListItem> tempList = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .Select(e => new SelectListItem
                {
                    Value = e.ToString(),
                    Text = e.ToString()
                })
                .ToList();
            if (addNotSelected)
            {
                tempList.Insert(0, new SelectListItem
                {
                    Value = string.Empty,
                    Text = "Not Selected"
                });
            }
            return tempList;
        }
    }
}
    
