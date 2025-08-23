using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GREPO
{
    public interface IRepository : IRepository<WSRef, Comment> { }
    public interface IRepository<T1, T2> : IDisposable             //  наследует  IDisposable
    {
        List<T1> getAllWSRef();                      // получить весь перечень Интернет-ресурсов 
        List<T2> getAllComment();                   // получить все комментарии  
        T2? GetCommentById(int Id);              // получить комментарий по его Id
        bool addWSRef(T1 wsRef);              // добавить новый  Интернет-ресурс  
        bool addComment(T2 comment);        // добавить комментарий   
    }

    public class WSRef
    {
        [Key]
        public int Id { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public int? Plus { get; set; }
        public int? Minus { get; set; }
        public List<Comment>? Comments { get; set; } // для FK
    }

    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public DateTime? Stamp { get; set; }
        public string? Commtext { get; set; }

        [ForeignKey("WSRef")] //  для FK ->  WSRef(PK) 
        public int WSrefId { get; set; }   //  для FK 
        public WSRef? WSref { get; set; }   //  для FK   
    }
}
