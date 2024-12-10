using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NoitirunApp.Domain.Common
{
    public abstract class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public bool Status { get; set; }

        /* 
         * Le Domain Events est un concept clé dans le Domain-Driven Design (DDD) permet de
         * capturer et de gérer des changements importants liés au domaine métier pour communiquer 
         * ces changements de manière explicite à d'autres parties du système qui pourraient en dépendre.
         * Exemple : Une facture a été payée. - Un utilisateur a été enregistré. - Une commande a été validée.
         * 
         * */
        private readonly List<BaseEvent> _domainEvents = new(); // _domainEvents : est une liste privée stocke les événements de domaine déclenchés par cette entité.

        [NotMapped]
        public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly(); //  DomainEvents Propriété exposée en lecture seule pour accéder à la liste des événements de domaine  (_domainEvents) sans permettre de les modifier directement. [NotMapped] pour indiquer à l'ORM de ne pas la persister dans la base de données.

        public void AddDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Add(domainEvent);
        }

        public void RemoveDomainEvent(BaseEvent domainEvent)
        {
            _domainEvents.Remove(domainEvent);
        }

        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}
