﻿using LiteDB;

namespace Acorisoft.FutureGL.MigaDB.Core
{
    /// <summary>
    /// 数据库
    /// </summary>
    public interface IDatabase
    {
        void DropCollection(string collectionName);

        T Delete<T>() where T : class;
        
        /// <summary>
        /// 获取数据库集合。
        /// </summary>
        /// <param name="collectionName">集合名</param>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <returns>返回一个数据类型。</returns>
        ILiteCollection<T> GetCollection<T>(string collectionName) where T : class;

        /// <summary>
        /// 获取指定的集合是否存在。
        /// </summary>
        /// <param name="collectionName">集合名</param>
        /// <returns>返回指定的集合是否存在。</returns>
        bool Exists(string collectionName);

        /// <summary>
        /// 获得布尔值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>返回结果</returns>
        bool Boolean(string key);

        /// <summary>
        /// 设置布尔值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>返回结果</returns>
        bool Boolean(string key, bool value);

        /// <summary>
        /// 获得字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string String(string key);

        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>返回结果</returns>
        string String(string key, string value);
        
        /// <summary>
        /// 获得字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        int? Int(string key);

        /// <summary>
        /// 设置字符串
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns>返回结果</returns>
        int Int(string key, int value);

        /// <summary>
        /// 获得值。
        /// </summary>
        /// <typeparam name="T">值类型。</typeparam>
        /// <returns>获得值。</returns>
        T Get<T>() where T : class;

        /// <summary>
        /// 设置值。
        /// </summary>
        /// <param name="instance">实例</param>
        /// <typeparam name="T">值类型。</typeparam>
        /// <returns>返回这个值本身。</returns>
        T Set<T>(T instance) where T : class;

        /// <summary>
        /// 设置值。
        /// </summary>
        /// <param name="instance">实例</param>
        /// <typeparam name="T">值类型。</typeparam>
        /// <returns>返回这个值本身。</returns>
        T Update<T>(T instance) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Upsert<T>(T instance) where T : class;
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        void IfSet<T>(T instance) where T : class;

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        bool Has<T>() where T : class;

        /// <summary>
        /// 升级当前数据库到指定版本。
        /// </summary>
        /// <param name="version">指定的版本。</param>
        void UpdateVersion(int version);

        /// <summary>
        /// 获取当前数据库版本
        /// </summary>
        /// <remarks>操作为非缓存操作，避免数据不一致。</remarks>
        public int Version { get; }
        
        /// <summary>
        /// 获取当前数据库属性。
        /// </summary>
        string DatabaseFileName { get; }

        /// <summary>
        /// 获取当前数据库属性。
        /// </summary>
        string DatabaseIndexFileName{ get; }

        /// <summary>
        /// 获取当前数据库属性。
        /// </summary>
        string DatabaseDirectory{ get; }
    }
}