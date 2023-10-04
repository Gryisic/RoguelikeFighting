using System.Text;

namespace Core.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder StartOpacity(this StringBuilder builder, float value) => builder.Append($"<alpha=#{value}>");
        
        public static StringBuilder EndOpacity(this StringBuilder builder) => builder.Append("</alpha>");
        
        public static StringBuilder StartSupString(this StringBuilder builder) => builder.Append("<sup>");
        
        public static StringBuilder EndSupString(this StringBuilder builder) => builder.Append("</sup>");
    }
}