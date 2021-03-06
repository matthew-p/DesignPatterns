﻿using System.Collections.Generic;
using Adapter.PersonalInformation;
using System.Data;
using System.IO;

namespace Adapter.Renderers
{
    public class QuestionAndAnswerRendererAdapter : IQuestionAndAnswerRendererAdapter
    {
        private DataRenderer dataRenderer;

        public string ListQuestionsAndAnswers(IEnumerable<QuestionAndAnswer> questionsAndAnswers)
        {
            var adapter = new QuestionsAndAnswersDbAdapter(questionsAndAnswers);
            dataRenderer = new DataRenderer(adapter);

            var writer = new StringWriter();
            dataRenderer.Render(writer);

            return writer.ToString();
        }

        internal class QuestionsAndAnswersDbAdapter : IDbDataAdapter
        {
            private readonly IEnumerable<QuestionAndAnswer> questionsAndAnswers;

            public QuestionsAndAnswersDbAdapter(IEnumerable<QuestionAndAnswer> questionsAndAnswers)
            {
                this.questionsAndAnswers = questionsAndAnswers;
            }

            public int Fill(DataSet dataSet)
            {
                var dataTable = new DataTable();
                dataTable.Columns.Add(new DataColumn("Question", typeof(string)));
                dataTable.Columns.Add(new DataColumn("Answer", typeof(string)));

                foreach (var qa in questionsAndAnswers)
                {
                    var row = dataTable.NewRow();
                    row[0] = qa.Question;
                    row[1] = qa.AnswerGiven;
                    dataTable.Rows.Add(row);
                }
                dataSet.Tables.Add(dataTable);
                dataSet.AcceptChanges();
                
                return dataTable.Rows.Count;
            }

            #region Not Implemented
            public IDbCommand DeleteCommand { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
            public IDbCommand InsertCommand { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
            public IDbCommand SelectCommand { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
            public IDbCommand UpdateCommand { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
            public MissingMappingAction MissingMappingAction { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
            public MissingSchemaAction MissingSchemaAction { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

            public ITableMappingCollection TableMappings => throw new System.NotImplementedException();

            public DataTable[] FillSchema(DataSet dataSet, SchemaType schemaType)
            {
                throw new System.NotImplementedException();
            }

            public IDataParameter[] GetFillParameters()
            {
                throw new System.NotImplementedException();
            }

            public int Update(DataSet dataSet)
            {
                throw new System.NotImplementedException();
            }
            #endregion
        }
    }
}
