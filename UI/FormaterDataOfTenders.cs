using FluentResults;
using Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI
{
    class FormaterDataOfTenders
    {
        public string FormatAllResults(Result<PageOfDeliveryJsonModel> pageOfDeliveryModel, Result<AdditionData> additionalData, Result<List<DocumentJsonModel>> documents)
        {
            return string.Concat(ErrorsWrap(pageOfDeliveryModel), ErrorsWrap(additionalData), ErrorsWrap(documents, FormatDocumentsData));
        }


        private string ErrorsWrap<T>(Result<T> result, Func<T, string> SuccessHundler)
        {
            if (result.Errors.Any())
            {
                return result.Errors.First().Message;
            }
            if (!result.IsFailed)
            {
                return SuccessHundler.Invoke(result.Value);
            }
            return "Неизвестная ошибка";
        }
        private string ErrorsWrap<T>(Result<T> result) => ErrorsWrap(result, x => x.ToString());

        private string FormatDocumentsData(List<DocumentJsonModel> documents)
        {
            StringBuilder result = new StringBuilder("    Список документов:\n");
            foreach (var document in documents)
            {
                result.AppendLine(document.ToString());
            }
            return result.ToString();

        }

    }

}

