namespace Digital.Infrastructure.Model.DocumentModel
{
    public class DocumentTypeModel
    {
        public string? Name { get; set; }
        public string? NormalizationName { get; set; }
        public bool IsActive { get; set; }
    }

    public class DocumentTypeCreateModel
    {

        public string Name { get; set; }
        //public bool IsActive { get; set; }

    }


    public class DocumentTypeUpdateModel
    {
        public string? Name { get; set; }
        public bool IsActive { get; set; }

    }
    public class DocumentTypeViewModel : DocumentTypeModel
    {
        public Guid Id { get; set; }
    }
}
