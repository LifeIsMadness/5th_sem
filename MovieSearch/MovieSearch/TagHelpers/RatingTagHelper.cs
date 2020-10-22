using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.TagHelpers
{


    public class RatingTagHelper: TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";

            var prevClass = context.AllAttributes.FirstOrDefault(a => a.Name == "class").Value;
            var content = await output.GetChildContentAsync();

            try
            {              
                var value = float.Parse(content.GetContent());

                if (value > 0 && value < 5)
                {
                    output.Attributes.SetAttribute("class", $"{prevClass} span-red");
                }
                else if (value >= 5 && value < 8)
                {
                    output.Attributes.SetAttribute("class", $"{prevClass} span-yellow");
                }
                else output.Attributes.SetAttribute("class", $"{prevClass} span-green");

            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
