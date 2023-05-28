namespace Acorisoft.Miga.Doc.Parts
{
    [Alias("page")]
    public class PageProperty : TextProperty
    {

        protected override InputProperty CreateInstanceOverride()
        {
            return new PageProperty();
        }
        
        protected internal override XElement GetElementOverride()
        {
            var element = new XElement("page");
            
            //
            //
            Write(element);

            element.Add(new XAttribute("fallback", Fallback ?? string.Empty));
            element.Add(new XAttribute("unit", Unit ?? string.Empty));

            return element;
        }
    }
}