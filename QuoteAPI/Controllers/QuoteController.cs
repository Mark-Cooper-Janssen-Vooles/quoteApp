using System.Collections.Generic;
using System.Linq;

namespace QuoteAPI
{
    public class QuoteController
    {
        private List<Quote> QuoteList { get; }
        private List<DraftQuote> DraftQuoteList { get; }

        public QuoteController(IEnumerable<Quote> quoteList, IEnumerable<DraftQuote> draftQuoteList)
        {
            QuoteList = quoteList.ToList();
            DraftQuoteList = draftQuoteList.ToList();
        }

        public IEnumerable<Quote> GetQuotes()
        {
            return QuoteList;
        }
        
        public IEnumerable<DraftQuote> GetDraftQuotes()
        {
            return DraftQuoteList;
        }

        public void SaveDraftQuote(DraftQuote draftQuote)
        {
            DraftQuoteList.Add(draftQuote);
        }
        
        // think about API for when draft quote finalised to normal quote
        public void AddQuote(DraftQuote draftQuote)
        {
            DraftQuoteList.Remove(draftQuote);
            QuoteList.Add(new Quote(draftQuote.Message));
        }
    }
}