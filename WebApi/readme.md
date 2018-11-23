
# callcriteria-api


## Hosting URL

`http://app.callcriteria-dev.com/webApi`


## Swagger

[http://app.callcriteria-dev.com/webApi/swagger](http://app.callcriteria-dev.com/webApi/swagger)




## API specs

All the dates should be formatted according to ISO 8601 format
(`2017-09-05` or `2017-02-09T00:00:00` -- we don't use timezones).


### General data types


```javascript

Period = {
    start: String,
    end: String,
}

Filters = {
    scorecards:   [Number],
    groups:       [String],
    agents:       [String],
    campaigns:    [String],
    QAs:          [String],
    missedItems:  [String],
    teamLeads:    [String],
    failedOnly:   Boolean,
    badCallsOnly: Boolean,
}

CallSystemData = {
    callId:                      Number,
    callType:                    String, // "call" or "website"
    receivedDate:                String, // ISO date
    callReviewStatus:            String, // "pending" or "reviewed" or "calibrated" or "edited" or "bad" or "disqualified"

    callAudioUrl:                String,
    callAudioLength:             Number, // seconds
    websiteUrl:                  String,

    scorecardId:                 Number,
    scorecardName:               String,
    scorecardFailScore:          Number,

    // for "reviewed" or "calibrated" or "edited"

    reviewDate:                  String,
    reviewerUserRole:            String,
    reviewerName:                String, // QA
    calibratorId:                Number,
    calibratorName:              String,

    agentScore:                  Number,
    callFailed:                  Boolean,
    missedItemsCount:            Number,

    reviewCommentsPresent:       Boolean,
    notificationCommentsPresent: Boolean,

    notificationId:              Number,
    notificationStatus:          String, // "none" or "notification" or "dispute"
    isNotificationOwner:         Boolean,

    // for "bad" or "disqualified"

    badCallReason:               String,
}

SimpleAnswerComment = {
    callId:         Number,
    commentId:      Number,
    commentText:    String,
}

SimpleQuestionAnswer = {
    answerId:      Number,
    answerText:    String,
    position:      Number, // seconds
    comments:      [SimpleAnswerComment], // optional for some requests
    customComment: String,
}

CompositeAnswerComment = {
    callId:         Number,
    questionId:     Number,
    commentId:      Number,
    commentText:        String,
    isChecked:      Boolean,
    position:       Number, // seconds
}

CompositeQuestionAnswer = {
    comments:       [CompositeAnswerComment],
    customComment:      String,
    position:       Number, // seconds
}

QuestionDetails = {
    callId:                         Number,
    questionId:                     Number,
    questionShortName:              String,
    questionSectionName:            String,
    questionType:                   String, // "simple" or "dynamic" or "composite"
    isRightAnswer:                  Boolean,
        isLinked:                       Boolean,
    isComposite:            Boolean,
    simpleQuestionAnswer:           SimpleQuestionAnswer, // for "simple" or "dynamic"
    compositeQuestionAnswer:        CompositeQuestionAnswer, // for "composite"
}

CallMetaData = {
    callDate:          String,
    agentName:         String,
    agentGroup:        String,
    campaign:          String,
    sessionId:         String,
    profileId:         String,
    prospectFirstName: String,
    prospectLastName:  String,
    prospectPhone:     String,
    prospectEmail:     String,
}

Group = {
    groupId: String,
    groupName: String,
}

Agent = {
    agentId: String,
    agentName: String,
}

Campaign = {
    campaignId: String,
    campaignName: String,
}

Scorecard = {
    scorecardId: Number,
    scorecardName: String,
    scorecardFailScore: Number,
}

QA = {
    qaId: String,
    qaName: String,
}

TeamLead = {
    teamLeadId: String,
    teamLeadName: String,
}

```


### POST /v2.3/dashboard/GetAvailableDashboardFilter (IMPLEMENTED WITH DIFFERENT FORMAT -- see Swagger)

Response:

```javascript

FiltersFormData = {
    rangeCalls: {
        total: Number,
        filtered: Number,
    },
    agents: [{
        agent: Agent,
        callsCount: Number,
        group: Group
    }],
    groups: [{
        group: Group,
        callsCount: Number,
        top3Agents: [Agent],
    }],
    campaings: [{
        campaign: Campaign,
        callsCount: Number,
    }],
    scorecards: [{
        scorecard: Scorecard,
        callsCount: Number,
    }],
    qas: [{
        qa: QA,
        callsCount: Number,
        teamLead: TeamLead,
    }],
    teamLeads: [{
        teamLead: TeamLead,
        callsCount: Number,
        top3QAa: [QA],
    }],
}

```


### POST /v2.3/dashboard/GetCalendarCounts

Request:

```javascript

GetCalendarCountsRequestData = {
    filters: Filters,
}

```

Response:

```javascript

GetCalendarCountsResponseData = {
    dayCalls: [{
        total: Number,
        filtered: Number,
        day: String,
    }], // data for last 100 days
}

```


### POST /v2.3/dashboard/GetAgregatedStatistics


Response:

```javascript

AggregatedStats = {
    totalCalls:        Number,
    totalFailedCalls:  Number,
    totalBadCalls:     Number,
    totalCallsSeconds: Number,
    totalAgents:       Number,
    avgAgentScore:     Number,
}

AgregatedStatisticsResponseData = {
    rangeStats:      AggregatedStats,
    comparisonStats: AggregatedStats,
}

```



### POST /v2.3/dashboard/GetCoachingQueue

Response:

```javascript


CoachingQueueCallDetails = {
    systemData:        CallSystemData,
    metaDataCallDate:  String,
    metaDataAgentName: String,
}

CoachingQueueResponseData = {
    calls:              [CoachingQueueCallDetails],
    totalNotifications: Number,
}

```


### POST /v2.3/dashboard/GetAgentRanking


Response:

```javascript

AgentMissedPoint = {
    questionShortName:  String,
    totalCalls:     Number, // total number of calls by agent where the question is present
    missedCalls:        Number, // number of calls by agent he failed the question
    questionType:       String, // "simple" or "dynamic" or "composite"
    isLinked:       Boolean,
    isComposite:        Boolean,
}

Agent = {
    id:               String,
    name:             String,
    groupNames:       [String], // agent can work on different groups during seleted period
    averageScore:     Number, // 0-100
    totalCalls:       Number, // calls by agent during selected period
    earliestCallDate: String,
    top3MissedPoints: [AgentMissedPoint],
}

AgentRankingResponseData = {
    agents: [Agent],
}


```



### POST /v2.3/dashboard/GetTopMissedItems


Response:

```javascript

Agent = {
    questionId:  Number, // duplicated field from parent object
    name:        String,
    totalCalls:  Number, // calls by agent within selected period where question is present
    missedCalls: Number, // calls by agent within selected period where he failed the question
}

TopMissedItemDetails = {
    question:           QuestionDetails,
    scorecardName:      String,
    questionSectionName:    String,
    totalCalls:         Number,
    missedCalls:        Number,
    top3Agents:         [Agent],
    questionType:       String, // "simple" or "dynamic" or "composite"
        isLinked:       Boolean,
    isComposite:        Boolean,
}

TopMissedItemsResponseData = {
    missedItems: [TopMissedItemDetails] // TopMissedItemDetails.question.comments whould be null
}
```


### POST /v2.3/dashboard/GetAvgScore

Request:

```javascript

AvgScoreRequestData = {
    range:   Period,
    filters: Filters,
}

```

Response:


```javascript

AverageDayScore = {
    averageScore: Number, // 0-100
    date:         String,
}

AvgScoreResponseData = {
    averageScores: [AverageDayScore],
}

```



### POST /v2.3/dashboard/GetCallDetails


Request:

```javascript

CallDetailsRequestData = {
    filters:    Filters,
    range:      Period,
    pagination: {
        pageNumber:   Number,
        itemsPerPage: Number,
    },
    sorting: {
        sortBy:    String, // column id
        sortOrder: String, // "asc" or "desc"
    },
}
```


Response:

```javascript
CallDetails = {
    systemData:      CallSystemData,
    metaData:        CallMetaData,
    callMissedItems: [QuestionDetails], // CallDetails.callMissedItems[].comments whould be null
}

CallDetailsResponseData = {
    calls:      [CallDetails],
    itemsTotal: Number,
}
```



### POST /v2.3/dashboard/GetCallShortInfo

Response:

```javascript

Role = {
    userRoleName: String,
    userRoleId:   String,
}

User = {
    userName: String,
    userId:   String,
}

UserInformation = {
    userRole: Role,
    userData: User,
}

NotificationComment = {
    openDate:       String, // ISO format
    closedDate:     String, // ISO format
    openedBy:       UserInformation,
    closedBy:       UserInformation,
    text:           String,
    id:             Number,
    noficationRole: Role,
}
SystemComment = {
    user:           UserInformation,
    commentDate:    String,
    text:           String,
    id:             Number,
}

NotificationInfo = {
    assignedTo:           Role,
    notificationStatus:   String, // "none" or "notification" or "dispute"
    notificationComments: [NotificationComment],
    systemComments:       [SystemComment],
    reassignOptions:      [Role],
    canClose:             Boolean,
}

GetCallShortInfoResponseData = {
    systemData:       CallSystemData,
    metaData:         CallMetaData,
    callMissedItems:  [QuestionDetails],
    notificationInfo: NotificationInfo,
}
```


### POST /v2.3/dashboard/SendNotification

Request:

```javascript

NotificationAction = {
    callId:         Number,
    text:           String,
    assignToRole:   Role,
    action:         String, // "comment" or "assign" or "close"
}

SendNotificationRequestData = NotificationAction
```



### POST /v2.3/dashboard/GetGroupPerfomance


Response:

```javascript

GroupPerformance = {
    groupName:          String, // 0-100
    groupId:            String,
    scorecardName:      String,
    currentPeriod:      PreiodPerformance,
    previousPeriod:     PreiodPerformance,
}

PreiodPerformance={
    callsCount:     Number,
    score:      String,
}
GroupPerformanceResponseData = {
    groups:     [GroupPerformance],
}


```
### POST /v2.3/dashboard/GetCampaignPerfomance


Response:

```javascript

CampaignPerformance = {
    campaignName:       String, // 0-100
    campaignId:         String,
    scorecardName:      String,
    currentPeriod:      PreiodPerformance,
    previousPeriod:     PreiodPerformance,
}

PreiodPerformance={
    callsCount:     Number,
    score:      String,
}
CampaignPerformanceResponseData = {
    campaigns:     [CampaignPerformance],
}


```


### POST /v2.3/dashboard/GetCallsLeftList

Request:

```javascript

GetCallsLeftRequestData = {
    range:   Period,
    filters: Filters,
}

```

Response:


```javascript

CallsLeft = {
    pendingCalls:   PendingCallsData,   
    pending:        Number,
    reviewed:       Number,
    badCalls:       Number,
    callDate:       String,
    scorecard:      Scorecard,
}

Scorecard = {
    scorecardId:      Number,
    scorecardName:    String,
}
PendingCallsData = {
    pendingCalls: [PendingCall],
}
PendingCall={
    receiveDate:    String,
    scorecardId:    Number,
}
```



### POST /calibration/GetCalibrationQueue


Response:

```javascript

Scorecard = {
    scorecardId:      Number,
    scorecardName:    String,
}

CalibrationInfo = {
    oldestCall:             String, // date
    pendingCalibrations:    Number,
    pendingReviewTime:      Number,
    scorecard:              Scorecard,
}

CalibrationQueueResponseData = {
    calibrations:     [CalibrationInfo],
}


```

### POST /calibration/GetCalibrationCalls


Response:

```javascript

CalibrationStatus = {
    reviewed:    Number,
    completed:   Number,
}

CalibrationCall = {
    calibrationId:  Number,
    callDate:       String, // date
    dateAdded:      String, // date
    ccSide:         CalibrationStatus,
    clientSide:     CalibrationStatus,
    status:         CalibrationStatus,
    phone:          String,
    scorecard:  Scorecard,
    callLength: Number,
    callType:   String,
    callId:     Number,
}

CalibrationCallsResponseData = {
    pending:     [CalibrationCall],
    completed:   [CalibrationCall]
}


```
### POST /v2.4/dashboard/GetMyPayInfo


Response:

```javascript

PaymentInfo: {
    totalCallTime              : Number,
    totalReviewTim             : Number,
    totalBadCallTime           : Number,
    totalBadCallReviewTime     : Number,
    baseRate                   : Number,
    adjustedRate               : Number,
    totalPay                   : Number,
    disputeCost                : Number,
    disputeCount               : Number,
    score                      : Number,
    callSpeed                  : Number,
}

ScorecardWeekInfo = {
    qaPaymentInfo: PaymentInfo,
    calibratorPaymentInfo: PaymentInfo,
    scorecard: Scorecard,
    weekEnd: String,
}

Week = {
    [ScorecardWeekInfo],
}

MyPay = {
    weeks: [Week],
    startDate: String,
}

```
### POST /v2.4/dashboard/GetQuestionInfo


Response:

```javascript

SimpleComment = {
    commentText:    String,
    commentId:      Number,
    total:          Number
}

SimpleAnswer = {
    comments:               [SimpleComment],
    answerText:             String,
    isRightAnswer:          Boolean,
    total:                  Number,
    totalCustomComments:    Number
}

SimpleQuestionStat = {
    answerList:    [SimpleAnswer]
}

CompositeComment = {
    commentText:    String,
    commentId:      Number,
    total:          Number
}

CompositeAnswerInfo = {
    totalCustomComments:    Number,
    answerText:             String
}

CompositeQuestionStat = {
    totalRight:         Number,
    rightAnswerInfo:    CompositeAnswerInfo,
    wrongAnswerInfo:    CompositeAnswerInfo,
    comments:           [CompositeComment] // these comment using as question template (not custom)
}

QuestionInfo = {
    total:                    Number,
    isComposite:              Boolean,
    isLinked:                 Boolean,
    questionId:               Number,
    questionName:             String,
    questionType:             String,
    sectionName:              String,
    simpleQuestionStat:       SimpleQuestionStat,
    compositeQuestionStat:    CompositeQuestionStat,
    scorecard:                Scorecard
}

```
### POST /v2.4/dashboard/GetSectionsInfo


Response:

```javascript
QuestionInfo ={
    qId:                Number,
    questionShortName:  String,
    totalRight:         Number,
    totalWrong:         Number,
    isComposite:        Boolean,
    isLinked:           Boolean,
    questionType:       String,
}
Section = {
    sectionInfo:    SectionInfo,
    questions:      [QuestionInfo]
}
SectionInfo = {
    sectionOrder:   Number,
    sectionId:      Number,
    sectionName:    String,
}
ScorecardInfo = {
    scorecard:    Scorecard,
    sections:     [Section]
}
GetSectionsInfo = [ScorecardInfo]

```

### POST /v2.4/dashboard/GetWebsiteStatistic

Response:

```javascript

GetWebsiteStatistic = {
    compliant:       Number,
    nonCompliant:    Number,
    bad:             Number,
    total:           Number,
}

```


### POST /v1.0/guidelines/UpdateScorecardStatus

Request:

```javascript

GetScorecardInfoRequestData = {
    scorecardid:    Number, // scorecardId
}
```
Response:
HttpStatus: 200

### POST /v1.0/guidelines/GetScorecardInfo

Request:

```javascript

GetScorecardInfoRequestData = {
    id:    Number, // scorecardId
}

```

Response:

```javascript

QuestionTemplateItem: {
    optionId:      Number,
    optionText:    String,
    checked:       Boolean,
}

CompositeQuestionInfo: {
    comments:    [QuestionTemplateItem],
    answers:     [Answer],
}

Answer: {
    answerText:       String,
    answerId:         Number,
    points:           Number, // should be added to agent score if isAnswered
    isRightAnswer:    Boolean,
}

Comment: {
    commentId:      Number,
    commentText:    String,
    points:         Number, // should be added to agent score if comment is checked
    checked:        Boolean
}

AnswerInfo = {
    answer:             Answer,
    commentRequired:    Boolean,
    comments:           [Comment],
    isAnswered:         Boolean,
    customComment:      String
}

SimpleQuestionInfo: {
    singleComment:    Boolean, // only one comment - not custom - can be checked (works like radiobutton)
    answers:          [AnswerInfo],
}

MetaDataItem: {
    name:     String,
    value:    String,
}

Instruction: {
    answerText:         String,
    instructionText:    String,
}

FAQ: {
    answerText:      String,
    questionText:    String,
}

QuestionInfo: {
    faqs:                     [FAQ], // need only on listen page (on guidlines maybe no) ???
    instructions:             [Instruction],
    questionId:               Number,
    isWide:                   Boolean,
    isComposite:              Boolean,
    singleComment:            Boolean,
    isLinked:                 Boolean, // only for "linkedAnswerId" and "linkedCommentId"
    questionType:             String, // "simple" or "dynamic" or "composite"
    linkedAnswerId:           Number, // answerId the question is linked to
    linkedCommentId:          Number, // commentId the question is linked to
    linkedMetaData:           MetaDataItem, // if questionType is "dynamic" should not be empty. Question appear when MetaDataItem from MetaData matching this object 
    linkedVisible:            Boolean, // question is visible until "linkedAnswerId" or "linkedCommentId" is checked
    qustionShortName:         String,
    commentAllowed:           Boolean,
    simpleQuestionInfo:       SimpleQuestionInfo, // not empty if "isComposite" is "false" - questionType is "simple" or "dynamic"
    compositeQuestionInfo:    CompositeQuestionInfo, // not empty if "isComposite" is "true"
}

Scorecard: {
    scorecardId:      Number,
    scorecardName:    String,
    scorecardApp:     String,
    scorecardType:    String // "audio", "website"
}

SectionInfo = {
    sectionOrder:   Number,
    sectionId:      Number,
    sectionName:    String,
}

Section: {
    sectionInfo:    SectionInfo,
    questions:      [QuestionInfo]
}

ScorecardInfo: {
    scorecard: Scorecard,
    sections: [Section]
}

GetScorecardInfo = {
    scorecardInfo:   ScorecardInfo,
    metaData:        [MetaDataItem],
}

```
### POST /v1.0/guidelines/UpdateMetadataInfo

Request:

```javascript

UpdateMetadataPayload = {
    id:               Number, // id from metadata
    metaDataItems:    [MetaDataItem],
}

```

Response:

```javascript

GetReviewInfo = {
    scorecardInfo:   ScorecardInfo, // from /guidelines/GetScorecardInfo
    metaData:        [MetaDataItem],
}

```
### POST /v1.0/guidelines/GetReviewInfo

Request:

```javascript

GetReviewInfoRequestData = {
    id:             Number, // review id
    scorecardId:    Number,
}

```

Response:

```javascript

GetReviewInfo = {
    scorecardInfo:   ScorecardInfo, // from /guidelines/GetScorecardInfo
    metaData:        [MetaDataItem],
}

```

### POST /v1.0/billing/GetBillingInfo

Request:

```javascript

GetBillingInfoRequestData: {
    // TODO - Stace, could you please provide request data if it
    // necessary
}

```

Response:

```javascript

BillableRate: {
    rate: Number,
    minutesFrom: Number,
    minutesTo: Number
}
BillingData: {
    date: String, // month and year for review
    currentBillableRate: Number, // payment per minute
    billableTime: Number, // seconds
    successfulTime: Number, // seconds
    badCallsTime: Number, // seconds
    transcriptMinutes: Number, // seconds
}
BillingResponseData: {
    transcriptRate: Number, // payment per transcribed minute
    cpmBillableRate: [BillableRate], // array of possible ratesObjects
    months: [BillingData],
    minimumMinutes: Number // if total < minimum minutes round up to minimum minutes for billing
}

```

### POST /v1.0/notification/GetNotificationCalls

Request:

```javascript

Filters = {
    apps:        [String], // array of filter ids
    supervisors: [String],
    scorecards:  [Number],
    QAs:         [String],
    teamLeads:   [String],
    calibrators: [String],
}

GetNotificationCallsRequestData: {
    filters:    Filters,
    range:      Period,
    pagination: {
        pagenum:   Number,
        pagerows: Number,
    },
    searchText: String,
    sorting: {
        sortBy:    String, // column id
        sortOrder: String, // "asc" or "desc"
    },
}

```

Response:

```javascript

Scorecard: {
    scorecardId:   Number,
    scorecardName: String,
}

Supervisor: {
    supervisorId:   Number,
    supervisorName: String,
}

Role = {
    userRoleName: String,
    userRoleId:   String,
}

User = {
    userName: String,
    userId:   String,
}

UserInformation = {
    userRole: Role,
    userData: User,
}

QuestionInfo ={
    questionId:         Number,
    questionShortName:  String,
    isComposite:        Boolean,
    isLinked:           Boolean,
    questionType:       String,
}

App: {
    appId: String, // ????? or Number
    appName: String,
}

Notification: {
    notificationId:     Number,
    scorecard:          Scorecard,
    callType:           String, // 'website', 'audio', 'chat'
    supervisor:         User,
    assignTo:           UserInformation,
    app:                App,
    openedDate:         String,
    closedDate:         String,
    reviewedDate:       String,
    reviewerName:       String,
    calibratorName:     String,
    teamLeadName:       String,
}

ScorecardNotification: {
    scorecard:          Scorecard,
    app:                App,
    totalCount:         Number, // total count of notifications
    openedCount:        Number, // open pending notifications count
    inProgressCount:    Number,
    closedCount:        Number,
    avgDaysOpen:        Number,
}

UserNotification: {
    scorecard:          Scorecard,
    app:                App,
    assignTo:           UserInformation,
    totalCount:         Number, // total count of notifications
    openedCount:        Number, // open pending notifications count
    inProgressCount:    Number,
    closedCount:        Number,
    avgDaysOpen:        Number,
}

NotificationCallsResponseData: {
    notifications: [Notification],
    total: Number, // total number of notifications for applied filters
}

```

### POST /v1.0/notification/GetNotificationsByScorecard

Request:

```javascript

NotificationsByScorecardResponseData: [ScorecardNotification]

```

### POST /v1.0/notification/GetNotificationsByUser

Request:

```javascript

NotificationsByUserResponseData: [UserNotification]

```

### POST /v1.0/notification/GetAvailableNotificationFilters

Request:

```javascript

Filters = {
    apps:        [String],
    supervisors: [String],
    scorecards:  [Number],
    QAs:         [String],
    teamLeads:   [String],
    calibrators: [String],
}

GetAvailableNotificationFiltersRequestData: {
    filters:    Filters,
    range:      Period,
    pagination: {
        pagenum:   Number,
        pagerows: Number,
    },
    searchText: String,
    sorting: {
        sortBy:    String, // column id
        sortOrder: String, // "asc" or "desc"
    },
}

```

Response:

```javascript

FilterItem: {
    count:  Number,
    id:     String,
    name:   String
}

Filters: {
    apps:        [FilterItem],
    supervisors: [FilterItem],
    scorecards:  [FilterItem],
    QAs:         [FilterItem],
    teamLeads:   [FilterItem],
    calibrators: [FilterItem],
}

Count: {
    day:      String,
    filtered: Number,
    total:    Number,
}

GetAvailableNotificationFiltersResponseData: {
    filters: Filters,
    counts: [Count],
}

```

### POST /v1.0/nidashboard/GetAgregatedStatistics


Response:

```javascript

AggregatedStats = {
    totalCalls:        Number,
    noOpportunity:     Number,
    noOvercomeOne:     Number,
    noOvercomeTwo:     Number,
}

AgregatedStatisticsResponseData = {
    rangeStats:      AggregatedStats,
    comparisonStats: AggregatedStats,
}

```

### POST /v1.0/nidashboard/GetPassedFailed


Response:

```javascript

AgregatedStatisticsResponseData = {
    passed: Number,
    failed: Number,
}

```

### POST /v1.0/nidashboard/GetObjections


Response:

```javascript

Reason: {
    reasonText: String,
    reasonId:   Number,
    total:      Number
}

ObjectionsResponseData = {
    reasons: [Reason],
    total:   Number
}

```

### POST /v1.0/nidashboard/GetGroups


Response:

```javascript

Agent = {
    agentId:   String,
    agentName: String,
}

AgentInfo: {
    agent:              Agent,
    totalCalls:         Number,
    failedCalls:        Number,
    noOvercomeOneCalls: Number,
    noOvercomeTwoCalls: Number,

}

Group: {
    agents: [AgentInfo]
}

GetGroupsResponseData = {
    groups: [Group],
}

```

### POST /v1.0/nidashboard/GetAvgScore

Request:

```javascript

AvgScoreRequestData = {
    range:   Period,
    filters: Filters,
}

```

Response:


```javascript

AverageDayScore = {
    averageScore: Number, // 0-100
    date:         String,
}

AvgScoreResponseData = {
    averageScores: [AverageDayScore],
}

```

### POST /v1.0/nidashboard/GetNICallDetails

Request:

```javascript

Filters = {
    agents:      [String],
    groups:      [String],
    scorecards:  [Number],
    QAs:         [String],
    teamLeads:   [String],
    campaigns:   [String],
}

GetNICallDetailsRequestData: {
    filters:    Filters,
    range:      Period,
    comparison: Period,
    pagination: {
        pagenum:   Number,
        pagerows:  Number,
    },
    searchText: String,
    sorting: {
        sortBy:    String, // column id
        sortOrder: String, // "asc" or "desc"
    },
}

```

Response:

```javascript

NICallDetailsResponseData = {
    calls:      [CallDetails],
    itemsTotal: Number,
}

```


## Call details table column fields


```javascript

tableColumns: {

    // first column, required, custom design

    callType: {
        title:  "Type",
        fields: ["CallSystemData.callType"],
    },


    // selectable columns

    receivedDate: {
        title:    "Received Date",
        fields:   ["CallSystemData.receivedDate"],
        sortable: true,
    },
    callAudioLength: {
        title:    "Duration",
        fields:   ["CallSystemData.callAudioLength"],
        sortable: true,
    },
    scorecardName: {
        title:  "Scorecard",
        fields: ["CallSystemData.scorecardName"],
        sortable: true,
    },
    reviewDate: {
        title:    "Review Date",
        fields:   ["CallSystemData.reviewDate"],
        sortable: true,
    },
    reviewerName: {
        title:    "Reviewer",
        fields:   ["CallSystemData.reviewerName"],
        sortable: true,
    },
    agentScore: {
        title:    "Score",
        fields:   ["CallSystemData.agentScore"],
        sortable: true,
    },
    missedItemsCount: {
        title:    "Missed Items",
        fields:   ["CallSystemData.missedItemsCount"],
        sortable: true, // technical difficulties
    },


    // selectable columns from meta data

    callDate: {
        title:    "Call Date",
        fields:   ["CallMetaData.callDate"],
        sortable: true,
    },
    agentName: {
        title:    "Agent",
        fields:   ["CallMetaData.agentName"],
        sortable: true,
    },
    agentGroup: {
        title:    "Group",
        fields:   ["CallMetaData.agentGroup"],
        sortable: true,
    },
    campaign: {
        title:    "Campaign",
        fields:   ["CallMetaData.campaign"],
        sortable: true,
    },
    sessionId: {
        title:    "Session Id",
        fields:   ["CallMetaData.sessionId"],
        sortable: true,
    },
    profileId: {
        title:    "Profile Id",
        fields:   ["CallMetaData.profileId"],
        sortable: true,
    },
    prospectFirstName: {
        title:    "First Name",
        fields:   ["CallMetaData.prospectFirstName"],
        sortable: true,
    },
    prospectLastName: {
        title:    "Last Name",
        fields:   ["CallMetaData.prospectLastName"],
        sortable: true,
    },
    prospectPhone: {
        title:    "Phone",
        fields:   ["CallMetaData.prospectPhone"],
        sortable: true,
    },
    prospectEmail: {
        title:    "Email",
        fields:   ["CallMetaData.prospectEmail"],
        sortable: true,
    },


    // required columns, sortable, custom design

    callReviewStatus: {
        title:  "Review Status",
        fields: [
            "CallSystemData.callReviewStatus",
            "CallSystemData.badCallReason", // for callReviewStatus values: "bad" or "disqualified"
        ],
    },
    result: {
        title:  "Result",
        fields: [
            "CallSystemData.callReviewStatus",
            "CallSystemData.callFailed", // for callReviewStatus values: "reviewed" or "calibrated" or "edited"
        ],
    }


    // last column, required, no label, custom design

    icons: {
        title:  null,
        fields: [
            "CallSystemData.callReviewStatus",
            "CallSystemData.reviewCommentsPresent", // blue icon
            "CallSystemData.notificationCommentsPresent", // yellow icon
            "CallSystemData.notificationStatus", // "none" or "notification" (yellow icon) or "dispute" (red icon)
            "CallSystemData.isNotificationOwner", // true (solid icon) or false (shaded icon)
        ], // for callReviewStatus values: "reviewed" or "calibrated" or "edited"
    },
}

```

###POST /v1.0/scheduling/GetUserSettings

Request:

```javascript

GetUserSettings: {
	userName:	Number
}
```

Response:

```javascript

ExtendedUserProfileModel = {
    userId:      	number,
    userName:		String,
	hoursPerWeek:	Number,
	daysPerWeek:	Number,
	prefStartHour:	Number
}

```
###POST /v1.0/scheduling/UpdateUserSettings

Request:

```javascript

UpdateUserSettings: {
	UserSettings:	ExtendedUserProfileModel
}

```
###POST /v1.0/scheduling/getAvailablePeriods

Response:

```javascript

ExtendedUserProfileModel = {
    userId:      	number,
    userName:		String,
	hoursPerWeek:	Number,
	daysPerWeek:	Number,
	prefStartHour:	Number
}

```
###POST /v1.0/scheduling/SetUserWorkingHour

Request:

```javascript

WorkingPeriod:{
	periodId:	Number,
	required:	Number,
	selected:	Number
}

RequieredQAModel:{
	appName:	String,
	scorecardId:	Number,
	dayDate: Date,
	[workingPeriods]:	WorkingPeriod

}

SetUserWorkingHour: {
[workingHours]:	RequieredQAModel
}
```

Response:

```javascript

ExtendedUserProfileModel = {
    userId:      	number,
    userName:		String,
	hoursPerWeek:	Number,
	daysPerWeek:	Number,
	prefStartHour:	Number
}

```

###POST /v1.0/scheduling/GetRequieredQAs

Request:

```javascript

ScQAsRequiredRQ:{
	appname:	String,
	scorecardId:	Number,
}

GetRequieredQAs = {
	scQAsRequiredRQ:	ScQAsRequiredRQ
}
```

Response:

```javascript

[RequieredQAModel]

```

###POST /v1.0/scheduling/UpdateRequieredQAs

Request:

```javascript

GetRequieredQAs = {
	[qAModel]:	RequieredQAModel
}
```

Response:

```javascript

[RequieredQAModel]

```

###POST /v1.0/scheduling/getInitialInfo


Response:

```javascript

Scorecards:{
	name:	String,
	id: Number
}

	App:{
	appName:	String,
	[scorecards]:	Scorecards
}

TimeModel:{
	id:	Number,
	start:	String,
	end:	String
}

GetInitialInfo = {
	[apps]:		App,
	[periods]:	TimeModel
}

```

