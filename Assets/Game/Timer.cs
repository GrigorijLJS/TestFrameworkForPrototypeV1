using UnityEngine;
using System.Collections;

//for UI Text, input fields etc.
using UnityEngine.UI;



namespace Prototype1v1
{

    public class Timer : MonoBehaviour
    {

        public Text learningText;
        public Scrollbar scrollbar1;

        //public int time;
		private Text timer;
        private Text hint;
        private Text score_text;
        private Text time_indicator_text;

        //private DiagnosticRules rulesObject = new DiagnosticRules();

		public MainScript mainScriptObj = new MainScript();


        public GameObject text_object;

        public GameObject hintTextObj;
        public GameObject scoreTextObj;
        public GameObject timeIndicatorTextObj;
        string temp_time_indicator = "";

		public GameObject QuestionManager;


        public GameObject AllPurposeGameObj;

		public string would_be_hint = "";
		public string the_question= "";


		private string current_activity_command = "";

		private string ongoing_learning_activity_name = "";

		//private bool last_learning_activity_limit_reached = false;

        //float time = 0;

		
		private ActivityMetrics quiz_activityObj = new ActivityMetrics();
		private string quiz_activity_name = "QuizActivity";
		
		private ActivityMetrics classification_activityObj = new ActivityMetrics();
		private string classification_activity_name = "ClassificationActivity";

		private ActivityMetrics temp_activity_container;

        public InputField inputField;
        public InputField inputField2;

        public Dropdown Dropdown_Menu;

        public Button start_button;

		
		private QuestionManager QManagObj = new QuestionManager();
		private bool new_question = false;
		private int new_points = 0;


        string activity_timer_start = "start";
        string activity_timer_pause = "pause";
        string activity_timer_unpause = "unpause";
        string activity_timer_stop = "stop";


        Vector3 move_pos_offscreen = new Vector3(1500.0f, 0, 0);
        Vector3 move_pos_onscreen = new Vector3(-1500.0f, 0, 0);


        
        void Start()
        {
            AllPurposeGameObj.transform.position += move_pos_offscreen;
            SetTheLearningText();
//            //so the objects of metrics are the same
//			mainScriptObj.rulesObject.playerMetricsObject = mainScriptObj.playerMetricsObject;



            timer = text_object.GetComponent<Text>();
			hint = hintTextObj.GetComponent<Text>();
            score_text = scoreTextObj.GetComponent<Text>();
            time_indicator_text = timeIndicatorTextObj.GetComponent<Text>();


			
			mainScriptObj = GameObject.Find("GameSystem").GetComponent<MainScript>();

            //start the counter for the time in-game
			mainScriptObj.playerMetricsObject.timeMetricsObject.StartAndStopTimeCounterSinceBeginningOfGame(true);

			/*mainScriptObj.playerMetricsObject.timeMetricsObject.CheckAndStoreInputGameTimeLimit(0, 0, 3);
			mainScriptObj.playerMetricsObject.timeMetricsObject.CheckAndStoreInputGameTimeLimit(0, 0, 5);*/



			//first activity
			if(!mainScriptObj.playerMetricsObject.gameActivitiesList.ContainsKey(quiz_activity_name))
			{
				
				mainScriptObj.playerMetricsObject.gameActivitiesList.Add(quiz_activity_name,quiz_activityObj);
			}

			if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name, out temp_activity_container))
			{
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 2);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 4);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 6);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 8);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 10);
				
				/*temp_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(2);
				temp_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(3);
				temp_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(4);*/

                ErrorMetrics temp_err_cont;
                /*temp_activity_container.EncouteredErrorsList.TryGetValue(temp_activity_container.errorRecognizeName
                    , out temp_err_cont);
                temp_err_cont.StoreInputThresholdsToNumberOfErrors(1);
                temp_err_cont = null;*/

                temp_activity_container.EncouteredErrorsList.TryGetValue(temp_activity_container.errorRecallName
                    , out temp_err_cont);
                temp_err_cont.StoreInputThresholdsToNumberOfErrors(3);
                temp_err_cont.StoreInputThresholdsToNumberOfErrors(7);
                temp_err_cont = null;

                /*temp_activity_container.EncouteredErrorsList.TryGetValue(temp_activity_container.errorImplementName
                    , out temp_err_cont);
                temp_err_cont.StoreInputThresholdsToNumberOfErrors(3);
                temp_err_cont = null;*/
			}

			//second activity
			if(!mainScriptObj.playerMetricsObject.gameActivitiesList.ContainsKey(classification_activity_name))
			{
				
				mainScriptObj.playerMetricsObject.gameActivitiesList.Add(classification_activity_name,classification_activityObj);
			}
			
			if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue
                (classification_activity_name, out temp_activity_container))
			{
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 3);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 5);
				temp_activity_container.CheckAndStoreInputTimeThreshold(0, 0, 7);


                ErrorMetrics temp_error_cont;
                temp_activity_container.EncouteredErrorsList.TryGetValue(temp_activity_container.errorClassifyName
                    , out temp_error_cont);
                temp_error_cont.StoreInputThresholdsToNumberOfErrors(5);
                temp_error_cont = null;

                temp_activity_container.EncouteredErrorsList.TryGetValue(temp_activity_container.errorImplementName
                    , out temp_error_cont);
                temp_error_cont.StoreInputThresholdsToNumberOfErrors(3);
                temp_error_cont = null;

				/*temp_learning_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(2);
				temp_learning_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(3);
				temp_learning_activity_container.StoreInputLimitToNumberOfTriesToSolveThisTask(4);*/
			}

            timeIndicatorTextObj.transform.position += move_pos_offscreen;
            scoreTextObj.transform.position += move_pos_offscreen;
            Dropdown_Menu.transform.position += move_pos_offscreen;
            inputField2.transform.position += move_pos_offscreen;

			temp_activity_container=null;

            //disable the second input field for now
            inputField2.DeactivateInputField();
            inputField2.interactable = false;

            //if (Input.GetKeyDown(KeyCode.Space))

            start_button.onClick.AddListener(delegate
            {

                Dropdown_Menu.transform.position += move_pos_onscreen;
                timeIndicatorTextObj.transform.position += move_pos_onscreen;
                scoreTextObj.transform.position += move_pos_onscreen;
                AllPurposeGameObj.transform.position += move_pos_onscreen;

                score_text.text = "";

                //move the reading material and GUI elements that are not needed
                start_button.transform.position += move_pos_offscreen;

                var slide = GameObject.Find("Scrollbar");
                slide.transform.position += move_pos_offscreen;

                var panel = GameObject.Find("Panel");
                panel.transform.position += move_pos_offscreen;

                var learning_text = GameObject.Find("LearningText");
                learning_text.transform.position += move_pos_offscreen;



                //get a new question
                Dropdown_Menu.options.Clear();
                QManagObj.NewQuestionForTheFirstActivity(ref the_question, ref Dropdown_Menu, mainScriptObj.the_score);

                //first task
                if (mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name,
                       out temp_activity_container) &&
                       temp_activity_container.activity_completed == false)
                {
                    CheckForStartOfActivity(ref temp_activity_container, activity_timer_start);
                    //temp_learning_activity_container=null;
                    ongoing_learning_activity_name = quiz_activity_name;
                }

                if (ongoing_learning_activity_name == quiz_activity_name)
                {
                    //Dropdown_Menu.captionText = Dropdown_Menu.captionText;

                    //Dropdown_Menu.options.Clear();
                    //Dropdown_Menu.OnSelect(Dropdown_Menu.onValueChanged);
                    Dropdown_Menu.onValueChanged.AddListener(delegate
                    {
                        if (Dropdown_Menu.options[Dropdown_Menu.value].text.ToString() != "Make your choice!")
                        {
                            new_points = 0;

                            //if the following int is 0, the answer is correct, if it is 1 error in recognizing happened
                            //if it is 2 error in recall was made, and 3 is for error in implementing 
                            int corectAnswerOrTypeOfError = QManagObj.CheckCorrectnessOfAnswer(
                                Dropdown_Menu.options[Dropdown_Menu.value].text, ref new_points);

                            /*int TempInt = Dropdown_Menu.value;
                            Dropdown_Menu.value = Dropdown_Menu.value + 1;
                            Dropdown_Menu.value = TempInt;*/

                            //Dropdown_Menu.options.Clear();
                            //Dropdown_Menu.value = 0;
                            //Dropdown_Menu.value = 0;

                            if (corectAnswerOrTypeOfError == 0)
                            {
                                //the player gets some points
                                mainScriptObj.the_score += new_points;

                                //update the diagnostic system about the score changes
                                mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                                //and check the rules for the score
                                mainScriptObj.CheckTheGameScoreAndItsRules(score_text);

                                DebugInput(Dropdown_Menu.options[Dropdown_Menu.value].text);
                                /*DebugInput(inputField.text);
                                inputField.text = "";*/
                                //the_question += "Correct!";
                            }
                            else
                            {//incorrect answer
                                //the player loses some points
                                mainScriptObj.the_score -= new_points;

                                //update the diagnostic system about the score changes
                                mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                                //and check the rules for the score
                                mainScriptObj.CheckTheGameScoreAndItsRules(score_text);

                                DebugInput("WAAAAAAAAAAH!!!" + inputField.text);
                                //mainScriptObj.CheckForErrors();

                                //check the type of the error
                                //if it is 1 error in recognizing happened, ID: errorInRecognizing
                                //if it is 2 error in recall was made, ID: errorInRecalling 

                                string error_information= mainScriptObj.playerMetricsObject.TrackAndStoreErrorForThisActivity
                                    (ongoing_learning_activity_name, "errorInRecalling");
                                Debug.LogError(error_information);
                                mainScriptObj.rulesObject.CheckRulesForErrors(ongoing_learning_activity_name, ref would_be_hint);
                                Debug.Log(would_be_hint);
                                
                                //ErrorMetrics temp_error_for_first;

                                /*if (corectAnswerOrTypeOfError == 1)
                                {

                                    temp_activity_container.EncouteredErrorsList
                                        .TryGetValue(temp_activity_container.errorRecognizeName, out temp_error_for_first);
                                    temp_error_for_first.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                                    string errorInfo = "ErrorInRecognizing timestamps: " +
                                        temp_error_for_first.error_time_stamps.Count + "#  ";
                                    for (int i = 0; i < temp_error_for_first.error_time_stamps.Count; i++)
                                    {
                                        errorInfo += temp_error_for_first.error_time_stamps[i] + "  ";
                                    }
                                    Debug.LogError(errorInfo);


                                    mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                                    hint.text = would_be_hint;
                                    //string errorinfo = errore + " | " + temp_container.getnumberoferrors() + " timestamps: ";
                                    //for (int i = 0; i < temp_container.getnumberoferrors(); i++)
                                    //{
                                    //    errorinfo += temp_container.error_time_stamps.elementat(i) + "  ";
                                    //}
                                    //debug.logerror(errorinfo);
                                }*/
                                //if (corectAnswerOrTypeOfError == 2)
                                //////{
                                //////    temp_activity_container.EncouteredErrorsList
                                //////        .TryGetValue(temp_activity_container.errorRecallName, out temp_error_for_first);
                                //////    temp_error_for_first.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                                //////    string errorInfo = "ErrorInRecalling timestamps: "
                                //////        + temp_error_for_first.error_time_stamps.Count + "#  ";
                                //////    for (int i = 0; i < temp_error_for_first.error_time_stamps.Count; i++)
                                //////    {
                                //////        errorInfo += temp_error_for_first.error_time_stamps[i] + "  ";
                                //////    }
                                //////    Debug.LogError(errorInfo);

                                //////    mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                                //////    hint.text = would_be_hint;
                                //////}
                                /*else if (corectAnswerOrTypeOfError == 3)
                                {
                                    temp_activity_container.EncouteredErrorsList
                                        .TryGetValue(temp_activity_container.errorImplementName, out temp_error_for_first);
                                    temp_error_for_first.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                                    string errorInfo = "errorInImplementing timestamps: "
                                        + temp_error_for_first.error_time_stamps.Count + "#  ";
                                    for (int i = 0; i < temp_error_for_first.error_time_stamps.Count; i++)
                                    {
                                        errorInfo += temp_error_for_first.error_time_stamps[i] + "  ";
                                    }
                                    Debug.LogError(errorInfo);

                                    mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                                    hint.text = would_be_hint;
                                }*/

                                //inputField.text = "";
                            }

                            //get a new question
                            if (QManagObj.NewQuestionForTheFirstActivity(ref the_question, ref Dropdown_Menu,
                                mainScriptObj.the_score))
                            {

                                
                                //reset the question var to empty
                                //the_question = "";

                                //wait a bit and start the new activity

                                /*Dropdown_Menu.options.Clear();
                                Dropdown_Menu.value = 0;*/


                                //move the dropdown to a place where it cannot be seen 
                                Vector3 temp = new Vector3(1000.0f, 0, 0);
                                Dropdown_Menu.transform.position += temp;

                                //move the first input field to a place where it cannot be seen 
                                //Vector3 temp = new Vector3(1000.0f, 0, 0);
                                inputField.transform.position += temp;

                                the_question = "";
                                the_question = "New activity will start soon! Please wait...\n\n\n\n\n";

                                if (mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name,
                                   out temp_activity_container) &&
                                   temp_activity_container.activity_completed == false)//if the first activity is not finished
                                {//but the requirements are met


                                    CheckForStartOfActivity(ref temp_activity_container, activity_timer_stop);//end the first activity
                                    temp_activity_container = null;//and empty the container
                                }

                                //deactivate the process of getting input during the "transfer"
                                //from one activity to the other
                                inputField.DeactivateInputField();
                                inputField.interactable = false;

                                //wait a bit
                                StartCoroutine(WaitFunction());
                            }
                        }

                    });
                    /*inputField.onEndEdit.AddListener(delegate
                    {
                        new_points = 0;

                        //if the following int is 0, the answer is correct, if it is 1 error in recognizing happened
                        //if it is 2 error in recall was made, and 3 is for error in implementing 
                        int corectAnswerOrTypeOfError = QManagObj.CheckCorrectnessOfAnswer(inputField.text, ref new_points);

                        if (corectAnswerOrTypeOfError == 0)
                        {
                            //the player gets some points
                            mainScriptObj.the_score += new_points;

                            //update the diagnostic system about the score changes
                            mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                            //and check the rules for the score
                            mainScriptObj.CheckTheGameScoreAndItsRules();

                            DebugInput(inputField.text);
                            inputField.text = "";
                        }
                        else
                        {//incorrect answer
                            //the player loses some points
                            mainScriptObj.the_score -= new_points;

                            //update the diagnostic system about the score changes
                            mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                            //and check the rules for the score
                            mainScriptObj.CheckTheGameScoreAndItsRules();

                            DebugInput("WAAAAAAAAAAH!!!" + inputField.text);
                            //mainScriptObj.CheckForErrors();

                            //check the type of the error
                            //if it is 1 error in recognizing happened, ID: errorInRecognizing
                            //if it is 2 error in recall was made, ID: errorInRecalling 

                            ErrorMetrics temp_error_for_first;
                            if (corectAnswerOrTypeOfError == 1)
                            {

                                temp_activity_container.EncouteredErrorsList
                                    .TryGetValue(temp_activity_container.errorRecognizeName, out temp_error_for_first);
                                temp_error_for_first.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                                string errorInfo = "ErrorInRecognizing timestamps: " +
                                    temp_error_for_first.error_time_stamps.Count + "#  ";
                                for (int i = 0; i < temp_error_for_first.error_time_stamps.Count; i++)
                                {
                                    errorInfo += temp_error_for_first.error_time_stamps[i] + "  ";
                                }
                                Debug.LogError(errorInfo);


                                mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                                hint.text = would_be_hint;
                                //string errorinfo = errore + " | " + temp_container.getnumberoferrors() + " timestamps: ";
                                //for (int i = 0; i < temp_container.getnumberoferrors(); i++)
                                //{
                                //    errorinfo += temp_container.error_time_stamps.elementat(i) + "  ";
                                //}
                                //debug.logerror(errorinfo);
                            }
                            else if (corectAnswerOrTypeOfError == 2)
                            {
                                temp_activity_container.EncouteredErrorsList
                                    .TryGetValue(temp_activity_container.errorRecallName, out temp_error_for_first);
                                temp_error_for_first.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                                string errorInfo = "ErrorInRecalling timestamps: "
                                    + temp_error_for_first.error_time_stamps.Count + "#  ";
                                for (int i = 0; i < temp_error_for_first.error_time_stamps.Count; i++)
                                {
                                    errorInfo += temp_error_for_first.error_time_stamps[i] + "  ";
                                }
                                Debug.LogError(errorInfo);

                                mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                                hint.text = would_be_hint;
                            }
                            else if (corectAnswerOrTypeOfError == 3)
                            {
                                temp_activity_container.EncouteredErrorsList
                                    .TryGetValue(temp_activity_container.errorImplementName, out temp_error_for_first);
                                temp_error_for_first.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                                string errorInfo = "errorInImplementing timestamps: "
                                    + temp_error_for_first.error_time_stamps.Count + "#  ";
                                for (int i = 0; i < temp_error_for_first.error_time_stamps.Count; i++)
                                {
                                    errorInfo += temp_error_for_first.error_time_stamps[i] + "  ";
                                }
                                Debug.LogError(errorInfo);

                                mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                                hint.text = would_be_hint;
                            }

                            inputField.text = "";
                        }
                        //get a new question
                        if (QManagObj.NewQuestionForTheFirstActivity(ref the_question, ref Dropdown_Menu))
                        {
                            //reset the question var to empty
                            //the_question = "";

                            //wait a bit and start the new activity

                            //move the first input field to a place where it cannot be seen 
                            Vector3 temp = new Vector3(1000.0f, 0, 0);
                            inputField.transform.position += temp;

                            the_question = "";
                            the_question = "New activity will start soon! Please wait...\n\n\n\n\n";

                            if (mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name,
                               out temp_activity_container) &&
                               temp_activity_container.activity_completed == false)//if the first activity is not finished
                            {//but the requirements are met


                                CheckForStartOfActivity(ref temp_activity_container, activity_timer_stop);//end the first activity
                                temp_activity_container = null;//and empty the container
                            }

                            //deactivate the process of getting input during the "transfer"
                            //from one activity to the other
                            inputField.DeactivateInputField();
                            inputField.interactable = false;

                            //wait a bit
                            StartCoroutine(WaitFunction());
                        }

                    });*/
                }
            });
            /*else if (ongoing_learning_activity_name==classification_activity_name)
            {

                inputField2.onEndEdit.AddListener(delegate
                {
                    DebugInput("Yaaaar!");
                });
            }*/
        }

        void SecondActivity()
        {
            if (mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name,
                   out temp_activity_container) &&
                   temp_activity_container.activity_completed)//if the first activity is finished
            {//start preparing for the second one


                DebugInput("Chaos sighted!");
                //CheckForStartOfActivity(ref temp_activity_container, activity_timer_stop);//end the first activity
                temp_activity_container = null;//empty the container

                //and extract the second activity
                mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(classification_activity_name,
                   out temp_activity_container);
                //temp_activity_container.StartStopOrPauseTimeOnActivityCounter(activity_timer_start);

                CheckForStartOfActivity(ref temp_activity_container, activity_timer_start);
                //temp_learning_activity_container=null;
                ongoing_learning_activity_name = classification_activity_name;

                //StartCoroutine(WaitFunction());
                //System.Threading.Thread.Sleep(3000);
                //Invoke("WaitFunction", 5.0f);

                /*inputField.ActivateInputField();
                inputField.interactable = true;*/

                //enable the second input field

                inputField2.transform.position += move_pos_onscreen;
                inputField2.ActivateInputField();
                inputField2.interactable = true;

                QManagObj.NewQuestionForTheSecondActivity(ref the_question, mainScriptObj.the_score);

                inputField2.onEndEdit.AddListener(delegate
                {
                    new_points = 0;

                    //if the following int is 0, the answer is correct, if it is 1 error in recognizing happened
                    //if it is 2 error in recall was made
                    int corectAnswerOrTypeOfError = QManagObj.CheckCorrectnessOfAnswerForSecond
                        (inputField2.text, ref new_points);

                    if (corectAnswerOrTypeOfError == 0)
                    {
                        //the player gets some points
                        mainScriptObj.the_score += new_points;

                        //update the diagnostic system about the score changes
                        mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                        //and check the rules for the score
                        mainScriptObj.CheckTheGameScoreAndItsRules(score_text);

                        DebugInput(inputField2.text);
                        inputField2.text = "";
                    }
                    else
                    {//incorrect answers
                        if (corectAnswerOrTypeOfError == 1)
                        {
                            //the player loses some points
                            mainScriptObj.the_score -= new_points;

                            //update the diagnostic system about the score changes
                            mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                            //and check the rules for the score
                            mainScriptObj.CheckTheGameScoreAndItsRules(score_text);

                            DebugInput("Red 'unz Go Fasta!!!!" + inputField2.text);
                            //mainScriptObj.CheckForErrors();

                            //check the type of the error

                            ////ErrorMetrics temp_error_for_second;
                            ////temp_activity_container.EncouteredErrorsList.TryGetValue(
                            ////    temp_activity_container.errorClassifyName, out temp_error_for_second);
                            ////temp_error_for_second.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                            ////string errorInfo = "errorInClassifying timestamps: " +
                            ////    temp_error_for_second.error_time_stamps.Count + "#  ";
                            ////for (int i = 0; i < temp_error_for_second.error_time_stamps.Count; i++)
                            ////{
                            ////    errorInfo += temp_error_for_second.error_time_stamps[i] + "  ";
                            ////}
                            ////Debug.LogError(errorInfo);

                            string error_information= mainScriptObj.playerMetricsObject.TrackAndStoreErrorForThisActivity
                                (ongoing_learning_activity_name, "errorInClassifying");
                            Debug.LogError(would_be_hint);
                            mainScriptObj.rulesObject.CheckRulesForErrors(ongoing_learning_activity_name, ref would_be_hint);
                            Debug.LogError(would_be_hint);

                            //mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                            hint.text = would_be_hint;

                            inputField2.text = "";
                        }
                        /*else if(corectAnswerOrTypeOfError == 2)
                        {
                            //the player loses some points
                            mainScriptObj.the_score -= new_points;

                            //update the diagnostic system about the score changes
                            mainScriptObj.playerMetricsObject.CheckAndStoreInputScore(mainScriptObj.the_score);
                            //and check the rules for the score
                            mainScriptObj.CheckTheGameScoreAndItsRules();

                            DebugInput("Red 'unz Go Fasta!!!!" + inputField2.text);
                            //mainScriptObj.CheckForErrors();

                            //check the type of the error

                            ////ErrorMetrics temp_error_for_second;
                            ////temp_activity_container.EncouteredErrorsList.TryGetValue(
                            ////    temp_activity_container.errorImplementName, out temp_error_for_second);
                            ////temp_error_for_second.ErrorMade(temp_activity_container.time_on_activity.Elapsed);

                            ////string errorInfo = "errorInImplementing timestamps: " +
                            ////    temp_error_for_second.error_time_stamps.Count + "#  ";
                            ////for (int i = 0; i < temp_error_for_second.error_time_stamps.Count; i++)
                            ////{
                            ////    errorInfo += temp_error_for_second.error_time_stamps[i] + "  ";
                            ////}
                            ////Debug.LogError(errorInfo);

                            string error_information = mainScriptObj.playerMetricsObject.TrackAndStoreErrorForThisActivity
                                (ongoing_learning_activity_name, "errorInClassifying");
                            Debug.LogError(would_be_hint);
                            mainScriptObj.rulesObject.CheckRulesForErrors(ongoing_learning_activity_name, ref would_be_hint);
                            Debug.LogError(would_be_hint);

                            //mainScriptObj.rulesObject.CheckRulesForErrors(ref temp_activity_container, ref would_be_hint);
                            hint.text = would_be_hint;

                            inputField2.text = "";
                        }*/
                    }
                    //get a new question
                    if (QManagObj.NewQuestionForTheSecondActivity(ref the_question, mainScriptObj.the_score))
                    {
                        
                        if (mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(classification_activity_name,
                           out temp_activity_container) &&
                           temp_activity_container.activity_completed == false)//if the second activity is not finished
                        {//but the requirements are met

                            CheckForStartOfActivity(ref temp_activity_container, activity_timer_stop);//end the second activity
                            temp_activity_container = null;//and empty the container


                            inputField2.DeactivateInputField();
                            inputField2.interactable = false;

                            //stop the counting of time in-game
                            mainScriptObj.playerMetricsObject.timeMetricsObject.StartAndStopTimeCounterSinceBeginningOfGame(false);

                            //give the final output to the player
                            the_question = " ---   GAME OVER! THANK YOU FOR PLAYING!   --- " +
                                "\n\n\nYour final score is: " + mainScriptObj.the_score+". ";
                            if (mainScriptObj.the_score>=150)
                            {
                                the_question += "PERFECT!!!";
                            }
                            else if (mainScriptObj.the_score >= 140 && mainScriptObj.the_score<150)
                            {
                                the_question += "Almost perfect!";
                            }
                            else if (mainScriptObj.the_score>=100 && mainScriptObj.the_score<140)
                            {
                                the_question += "Good.";
                            }
                            else if (mainScriptObj.the_score >= 50 && mainScriptObj.the_score < 100)
                            {
                                the_question += "Not bad.";
                            }
                            else if (mainScriptObj.the_score < 50)
                            {
                                the_question += "You really need to work on your knowledge about the geography of Europe.";
                            }

                            the_question += "\n\n\n\n";

                            mainScriptObj.logObj.WriteLog();
                        }
                    }
                });

                //end the game and show the score?????
                
            }
        }

        IEnumerator WaitFunction()
        {
            //wait for several seconds
            yield return new WaitForSeconds(5);

            ongoing_learning_activity_name = classification_activity_name;
            //then start the second activity
            SecondActivity();

        }


		private void DebugInput(string the_text)
		{
			Debug.Log ("Input: " + the_text);

		}
		
		
		
		void Update()
        {
			timer.text = the_question+ "\nTime since start of game(to be removed) "+
				mainScriptObj.playerMetricsObject.timeMetricsObject.time_since_beginning_of_game.Elapsed.ToString();

			//CheckForStartOfActivity(ref temp_learning_activity_container);

			//if(ongoing_learning_activity_name=="")
            //{
            //    if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(quiz_activity_name, 
            //       out temp_activity_container) && 
            //       temp_activity_container.activity_completed==false)
            //    {
            //        CheckForStartOfActivity(ref temp_activity_container);
            //        //temp_learning_activity_container=null;
            //    }
            //    else if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue(classification_activity_name, 
            //       out temp_activity_container) && 
            //       temp_activity_container.activity_completed==false)
            //    {
            //        CheckForStartOfActivity(ref temp_activity_container);

            //        if(new_question)
            //        {
            //            new_question=false;
            //            QManagObj.NewQuestionForTheFirstActivity(ref the_question);
            //        }
            //        //temp_learning_activity_container=null;
            //    }
            //}
			/*else
			{
				CheckForStartOfActivity(ref temp_learning_activity_container);
			}*/
				
//            timer.text = "Time since start " +
//				mainScriptObj.playerMetricsObject.timeMetricsObject.GetCountTimeSinceBeginningOfGame().ToString()
//					+"\ntime-on-task "+ mainScriptObj.playerMetricsObject.activityMetricsObject.GetTimeOnTask().ToString();

			//if(mainScriptObj.playerMetricsObject.learningActivitiesList.TryGetValue(learning_activity_name_A, out temp_learning_activity_container))
			if(temp_activity_container!=null)
            {

				timer.text +="\ntime-on-activity(to be removed) "+
					temp_activity_container.time_on_activity.Elapsed.ToString();

				mainScriptObj.rulesObject.CheckRulesForActivityMetrics(ref temp_activity_container,
                   ref temp_time_indicator);
                time_indicator_text.text = temp_time_indicator;
			
			}

			
			//mainScriptObj.rulesObject.CheckRulesForTimeMetrics(ref would_be_hint);
			
			//mainScriptObj.rulesObject.CheckRulesForErrors(ref would_be_hint);

			hint.text = would_be_hint;


           
        }

		/*void PickOneLearningActivity(ref temp_learning_activity_container)
		{

		}*/

//		void CheckRulesForActivityMetrics()
//		{
//			/* //is limit reached for the time since start
//			mainScriptObj.rulesObject.CompareCurrentGameTimeWithGameTimeLimit();
//			//if(mainScriptObj.rulesObject.GetGameTimeLimitReached())
//			if(mainScriptObj.rulesObject.game_time_limit_reached)
//            {
//                timer.text = "Game Over, man! Game Over!";
//				mainScriptObj.rulesObject.game_time_limit_reached=false;
//                //return;
//            }*/
//			
//			//			if(!last_task_limit_reached)
//			{
//				//is limit reached for time-on-task
//				//!!!!!!!!HAVE A CHECK AROUND THIS SO IT CAN BE SKIPPED IF NOT NEEDED
//				if(mainScriptObj.playerMetricsObject.activityMetricsObject.current_task_time_limit_index+1<=
//				   mainScriptObj.playerMetricsObject.activityMetricsObject.time_on_activity_thresholds.Count)
//				{
//					
//					mainScriptObj.rulesObject.CompareCurrentTaskTimeWithTaskTimeLimit();
//				}
//				
//				//			if (mainScriptObj.rulesObject.GetTaskTimeLimitReached())
//				/*if (mainScriptObj.playerMetricsObject.activityMetricsObject.GetTask_time_limit_compared
//				    (mainScriptObj.playerMetricsObject.activityMetricsObject.current_task_time_limit_index))*/
//				{
//					
//					//mainScriptObj.rulesObject.task_time_limit_reached=false;
//					//return;
//					
//					//this might need to be moved; not appropriate here?
//					if(mainScriptObj.playerMetricsObject.activityMetricsObject.current_task_time_limit_index+1<=
//					   mainScriptObj.playerMetricsObject.activityMetricsObject.time_on_activity_thresholds.Count)
//					{
//						if (mainScriptObj.playerMetricsObject.activityMetricsObject.GetTask_time_limit_compared
//						    (mainScriptObj.playerMetricsObject.activityMetricsObject.current_task_time_limit_index))
//						{
//							
//							hint.text = "Task Over, man! Task Over! #"+mainScriptObj.
//								playerMetricsObject.activityMetricsObject.current_task_time_limit_index;
//							
//							mainScriptObj.playerMetricsObject.activityMetricsObject.current_task_time_limit_index++;
//						}
//						
//					}
//					else 
//					{
//						last_task_limit_reached=true;
//					}
//				}
//			}
//		}

		void CheckForStartOfActivity(ref ActivityMetrics temp_learning_activity_container, string activity_command)
        {
			//ActivityMetrics temp_container;

            //if (Input.GetKeyDown(KeyCode.T) == true)
            {

				/*if(mainScriptObj.playerMetricsObject.gameActivitiesList.TryGetValue
				   (learning_activity_name_A, out temp_learning_activity_container))*/
				//if(temp_learning_activity_container!=null)
				{
					//if (current_activity_command=="" && Input.GetKeyDown(KeyCode.T) == true)
                    if(activity_command==activity_timer_start)
					{//start the counting of time-on-activity
						//temp_learning_activity_container.IncrementNumberOfTriesToSolve();
						temp_learning_activity_container.StartStopOrPauseTimeOnActivityCounter(activity_timer_start);
						//Debug.LogError(mainScriptObj.playerMetricsObject.activityMetricsObject.GetTimeOnTask());
						//Debug.LogError(mainScriptObj.playerMetricsObject.activityMetricsObject.time_on_activity.Elapsed.ToString());
						current_activity_command = activity_timer_start;
					}
					/*if(current_activity_command==activity_timer_start &&
					   temp_learning_activity_container.time_on_activity.IsRunning)*/
                    if (activity_command == activity_timer_pause && 
                        temp_learning_activity_container.time_on_activity.IsRunning)//if the activity is ongoing and the 
					{//time counter is running 
						//if (Input.GetKeyDown(KeyCode.P) == true)
						{
							current_activity_command = activity_timer_pause;
							temp_learning_activity_container.StartStopOrPauseTimeOnActivityCounter(activity_timer_pause);
						}

					}
					//if(current_activity_command==activity_timer_pause)
                    if (activity_command == activity_timer_unpause)
					{        
						//if(Input.GetKeyDown(KeyCode.U) == true)
						{
							temp_learning_activity_container.StartStopOrPauseTimeOnActivityCounter(activity_timer_unpause);
							current_activity_command = activity_timer_start;
						}
					}
					//if(Input.GetKeyDown(KeyCode.A) == true)
                    if (activity_command == activity_timer_stop)
					{//stop the counting of time-on-activity
						current_activity_command = "";
						temp_learning_activity_container.StartStopOrPauseTimeOnActivityCounter(activity_timer_stop);
						new_question=true;
					}
				}
                
            }
        }

        //this learning text is based on information which was extracted from various Wikipedia pages and  
        //EuroVoc (the EU's multilingual thesaurus)
        void SetTheLearningText()
        {
            scrollbar1.value = 1f;

            learningText.text = "\n\n\nHello! With this little game we want to help you learn more about the geography" +
            " of the \"old continent\" - Europe and its countries. You might ask yourself - how is this useful?" +
            " Well, imagine you are going on a trip through Europe and you want to visit some/most, or even just one," +
            " of the countries on the continent. Now, in order to prepare you for the journey we made a collection" +
            " of interesting and important information that you might need when interacting with the locals. Then you can" +
            " show off your knowledge and rise above the average tourist. " + 
            "First of all, let us start with some numbers. There are aproximatelly 50 countries in Europe and exactly 28" +
            " of them are part of the European Union. " +
            "The following is a list of some countries in Europe along with their capitals." +
            " Be sure to read and remember which capital goes with which country, in case you didn't know this already. \n" +

                            "\nCountry - Capital\n" +

                            "\nAlbania - Tirana" +
                            "\nAndorra - Andorra la Vella" +
                            "\nArmenia - Yerevan" +
                            "\nAustria - Vienna" +
                            "\nAzerbaijan - Baku" +
                            "\nBelarus - Minsk" +
                            "\nBelgium - Brussels" +
                            "\nBosnia and Herzegovina - Sarajevo" +
                            "\nBulgaria - Sofia" +
                            "\nCroatia - Zagreb" +
                            "\nCyprus - Nicosia" +
                            "\nCzech Republic - Prague" +
                            "\nDenmark - Copenhagen" +
                            "\nEstonia - Tallinn" +
                            "\nFinland - Helsinki" +
                            "\nFrance - Paris" +
                            "\nGeorgia - Tbilisi" +
                            "\nGermany - Berlin" +
                            "\nGreece - Athens" +
                            "\nHungary - Budapest" +
                            "\nIceland - Reykjavik" +
                            "\nIreland - Dublin" +
                            "\nItaly - Rome" +
                            "\nKazakhstan - Astana" +
                            "\nLatvia - Riga" +
                            "\nLiechtenstein - Vaduz" +
                            "\nLithuania - Vilnius" +
                            "\nLuxembourg - Luxembourg" +
                            "\nMacedonia - Skopje" +
                            "\nMalta - Valletta" +
                            "\nMoldova - Kishinev" +
                            "\nMonaco - Monaco" +
                            "\nMontenegro - Podgorica" +
                            "\nNetherlands - Amsterdam" +
                            "\nNorway - Oslo" +
                            "\nPoland - Warsaw" +
                            "\nPortugal - Lisbon" +
                            "\nRomania - Bucharest" +
                            "\nRussia - Moscow" +
                            "\nSan Marino - San Marino" +
                            "\nSerbia - Belgrade" +
                            "\nSlovakia - Bratislava" +
                            "\nSlovenia - Ljubljana" +
                            "\nSpain - Madrid" +
                            "\nSweden - Stockholm" +
                            "\nSwitzerland - Bern" +
                            "\nTurkey - Ankara" +
                            "\nUkraine - Kiev" +
                            "\nUnited Kingdom - London\n\n"


            + "\n\nAn interesting fact to note about the continent itself is that some consider it "
            + "to be one of the largest peninsulas on Earth."
            + " Speaking of peninsulas, did you know that the four largest ones in Europe (ordered from "
            + "largest to smallest) are the Scandinavian, Balkan, Iberian, and Italian? The largest peninsula "
            + "is shared by the following countries: Norway, Sweden, and Finland. The Balkan peninsula is comprised"
            + " of: Albania, Bulgaria, Bosnia and Herzegovina, Croatia, Greece, Italy, Kosovo, Macedonia, "
            + "Montenegro, Serbia, Slovenia, Romania, and Turkey. "
            + "Note that there is a difference between the Balkan peninsula and the Balkans, the latter being"
            + " a political region which does not include all of the previously mentioned countries. Confusing, right?"
            + " The Iberian peninsula, on the other hand, is shared by three countries: Spain, Portugal, and Andorra. "
            + "And last, but definitly not least, the Italian Peninsula (also known as Apennine Peninsula)"
            + " covers: Italy, the Vatican, and San Marino. There are many more peninsulas on the continent "
            + "but this game will only focus on the four major ones that you just read about. "

            + "\n\nNow let us discuss one possible way in which the countries in Europe can be grouped. "
            + "There are many ways of dividing the continent into regions, but this game will focus on the following ones: "
            +"Northern Europe, Western Europe, Eastern Europe, and Southern Europe. We will take a look at some imaginary "
            + "borders for each of them and help you form a rough image of the regions. Northern Europe"
            +" is comprised of the countries that lay north of the following countries: United Kingdom, the Netherlands, "
            + "Germany, Poland; and to the north-west of the line made from the borders of Russia and Belarus."
            + " Eastern Europe lies to the east of the line made by the borders of Germany, Austria, and Italy; to the north "
            + "of Greece and Turkey; and to the south/south-east of the borders of Finland, Estonia, Latvia, and Lithuania. "
            + "The countries of Southern Europe lay to the south of the borders of France, Switzerland, Albania, Macedonia,"
            +"and Bulgaria. Western European countries are to the north of Spain and Italy; to the west of Poland, Czech Republic"
            +", Slovakia, and Hungary; and to the south of Denmark and Iceland. "

            + "\n\n";
        }



		
    }
}