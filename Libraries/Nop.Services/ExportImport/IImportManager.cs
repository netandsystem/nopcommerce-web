﻿using Nop.Core.Domain.Catalog;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using static Nop.Services.ExportImport.ImportManager;

namespace Nop.Services.ExportImport
{
  /// <summary>
  /// Import manager interface
  /// </summary>
  public partial interface IImportManager
  {
    /// <summary>
    /// Import products from XLSX file
    /// </summary>
    /// <param name="stream">Stream</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task ImportProductsFromXlsxAsync(Stream stream);

    /// <summary>
    /// Import newsletter subscribers from TXT file
    /// </summary>
    /// <param name="stream">Stream</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the number of imported subscribers
    /// </returns>
    Task<int> ImportNewsletterSubscribersFromTxtAsync(Stream stream);

    /// <summary>
    /// Import states from TXT file
    /// </summary>
    /// <param name="stream">Stream</param>
    /// <param name="writeLog">Indicates whether to add logging</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the number of imported states
    /// </returns>
    Task<int> ImportStatesFromTxtAsync(Stream stream, bool writeLog = true);

    /// <summary>
    /// Import manufacturers from XLSX file
    /// </summary>
    /// <param name="stream">Stream</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task ImportManufacturersFromXlsxAsync(Stream stream);

    /// <summary>
    /// Import categories from XLSX file
    /// </summary>
    /// <param name="stream">Stream</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    Task ImportCategoriesFromXlsxAsync(Stream stream);

    #region NaS Code
    Task<(List<SkuPicture> productsUpdatedSP, List<SkuPicture> productsRejectedSP, List<Product> productsUpdated)> ImportProductsPicturesFromSkuPictureAsync(IList<SkuPicture> skuPictureList);

    #endregion
  }
}
