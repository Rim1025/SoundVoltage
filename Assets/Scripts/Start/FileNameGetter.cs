using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Start
{
    public class FileNameGetter
    {
        private List<string> _fileName;
    
        /// <summary>
        /// �w��t�H���_���̃t�@�C�������擾
        /// </summary>
        /// <param name="_filePath">�t�H���_�p�X</param>
        /// <returns>�擾�����f�[�^</returns>
        public List<string> GetFileName (string _filePath)
        {
            _fileName = new List<string>();

            // �f�B���N�g�������݂��邩�m�F
            if (Directory.Exists(_filePath))
            {
                // �f�B���N�g�����̃t�@�C�����擾
                string[] _files = Directory.GetDirectories(_filePath);

                // �t�@�C�����݂̂����X�g�ɒǉ�
                foreach (string _file in _files)
                {
                    _fileName.Add(Path.GetFileName(_file));
                }
            }
            else
                Debug.LogError("�w�肳�ꂽ�t�H���_��������܂���: " + _filePath);
            return _fileName;
        }
    }
}