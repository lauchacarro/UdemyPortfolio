using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UdemyPortfolio.Models.Udemy;

namespace UdemyPortfolio.Models.Paginator
{
    public class CertificatePaged : PagedResultBase, IList<Certificate>, ICloneable
    {
        private readonly IList<Certificate> _certificates;

        public CertificatePaged() : this(new List<Certificate>())
        {

        }

        public CertificatePaged(IEnumerable<Certificate> certificates)
        {
            _certificates = certificates.ToList() ?? new List<Certificate>();
            base.RowCount = this.Count;
            base.CurrentPage = 1;
        }

        public Certificate this[int index] { get => _certificates[index]; set => _certificates[index] = value; }

        public int Count => _certificates.Count;

        public bool IsReadOnly => _certificates.IsReadOnly;

        public void Add(Certificate item)
        {
            _certificates.Add(item);
            base.RowCount = this.Count;
        }

        public void Add(IEnumerable<Certificate> certificates)
        {
            foreach (var item in certificates)
            {
                Add(item);
            }
        }

        public void Clear() => _certificates.Clear();

        public bool Contains(Certificate item) => _certificates.Contains(item);

        public void CopyTo(Certificate[] array, int arrayIndex) => _certificates.CopyTo(array, arrayIndex);

        public IEnumerator<Certificate> GetEnumerator() => _certificates.GetEnumerator();

        public int IndexOf(Certificate item) => _certificates.IndexOf(item);

        public void Insert(int index, Certificate item) => _certificates.Insert(index, item);

        public bool Remove(Certificate item) => _certificates.Remove(item);

        public void RemoveAt(int index) => _certificates.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => _certificates.GetEnumerator();
        public object Clone()
        {
            CertificatePaged clone = new CertificatePaged
            {
                CurrentPage = base.CurrentPage
            };
            return clone;
        }
    }
}
