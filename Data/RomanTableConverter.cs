using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreateKeymappingHtml.Data {
    class RomanTableConverter {
        const string cell = "<table><tr><td class=\"input\">{0}</td><td class=\"output\">{1}</td><td class=\"next\">{2}</td></tr></table>";
        const string emptyCell = "<div>&nbsp;</div>";

        public string Convert(string data) {
            if (0 == data.Trim().Length) {
                return emptyCell;
            }

            var map = data.Split('\t');
            return string.Format(cell,
                0 < map.Length ? map[0].Trim() : "",
                1 < map.Length ? map[1].Trim() : "",
                2 < map.Length ? map[2].Trim() : ""
                );
        }

    }
}
