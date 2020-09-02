import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { IAppThunkAction } from "./";
import { UserInviteRequestViewModel } from '../models/UserInviteRequestViewModel';

export interface IInviteManagerComponentState {
    isLoading: boolean;
    result: IInvitesData;
    isEmailSent: boolean;
    isFailEmail: boolean;
    sentId:number;
    summary:InviteSummary
}

export interface IInvite {  
    userEmail: string;
    isUsed: boolean;  
    userInviteCode : string;
    id: number
}

export interface IInvitesData {
    invites: IInvite[]; 
}

export interface InviteSummary
{
    totalMails:number;
    usedMails:number;
    unUsedMails:number;
}

interface IRequestInviteManagerData {
    type: "REQUEST_INVITE_DATA";
}

interface IReceiveInviteManagerData {
    type: "RECEIVE_INVITE_DATA";
    result: IInvitesData;
}
interface ISendInviteEmail {
    type: "SENT_INVITE_EMAIL"; 
    sendId:number;
   
}

interface IInviteSummaryDate
{
    type: "INVITE_SUMMARY"; 
    summary:InviteSummary;
}



interface IFailInviteEmail {
    type: "FAIL_INVITE_EMAIL";
}

type KnownAction = IRequestInviteManagerData | IReceiveInviteManagerData | ISendInviteEmail | IFailInviteEmail|IInviteSummaryDate;

export const actionCreators = {
    requestInviteData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        
        const fetchTask = fetch(`api/invite/all`)
            .then((response) => response.json() as Promise<IInvitesData>)
            .then((data) => {
                dispatch({ type: "RECEIVE_INVITE_DATA", result: data, });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_INVITE_DATA" });

    },
    InviteSummaryData: (): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        
        const fetchTask = fetch(`api/invitesummary`)
            .then((response) => response.json() as Promise<InviteSummary>)
            .then((data) => {
               
                dispatch({ type: "INVITE_SUMMARY", summary: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_INVITE_DATA" });

    },
    searchInvitedata: (searchstring: string): IAppThunkAction<KnownAction> => (dispatch, getState) => {
        
        const fetchTask = fetch(`api/invitesearch/${searchstring}`)
            .then((response) => response.json() as Promise<IInvitesData>)
            .then((data) => {
                
                dispatch({ type: "RECEIVE_INVITE_DATA", result: data });
            });

        addTask(fetchTask);
        dispatch({ type: "REQUEST_INVITE_DATA" });
    },


    navigateToEdit:(obj): IAppThunkAction<KnownAction> => (dispatch,getHeapStatistics) => {
        localStorage.setItem("invite",JSON.stringify(obj.row));
    },
    sendEmail:(obj): IAppThunkAction<KnownAction> => (dispatch,getHeapStatistics) => {
    
       let model = new UserInviteRequestViewModel();
       model.email = obj.original.userEmail;
       model.id = obj.original.id;
       model.inviteCode = obj.original.userInviteCode;
       model.isUsed = obj.original.isUsed;

        var sendId=obj.original.id;
        const fetchTask = fetch(`api/invite/sendemail`, {
            method: 'POST',
            body: JSON.stringify(model),
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then((response: any) => {
                if (response.ok) {   
                      
                           
                    dispatch({ type: "SENT_INVITE_EMAIL",sendId:sendId });
                }
                else 
                {
                    dispatch({ type: "FAIL_INVITE_EMAIL" });
                }
            });

        addTask(fetchTask);
    }
};


const inviteManagerData: IInvitesData = {
    invites: null,
 };
const invitesummarydata:InviteSummary=
{
    usedMails:0,
    unUsedMails:0,
    totalMails:0
}

const unloadedState: IInviteManagerComponentState = {
 
    result: inviteManagerData,
    isLoading: false,
    isEmailSent: false,
    isFailEmail: false,
    sentId:0,
    summary:invitesummarydata
};

export const reducer: Reducer<IInviteManagerComponentState> = (state: IInviteManagerComponentState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    switch (action.type) {
        case "REQUEST_INVITE_DATA":
            return {
                result: state.result,
                isLoading: true,
                isEmailSent: false,
                isFailEmail: false,
            sentId:state.sentId,
            summary:state.summary
            };
        case "RECEIVE_INVITE_DATA":
            return {
                result: action.result,
                isLoading: false,
                isEmailSent: false,
                isFailEmail: false,
                sentId:state.sentId,
                summary:state.summary
            };
        case "SENT_INVITE_EMAIL":
      
            return {
                result: state.result,
                isLoading:false,
                isEmailSent: true,
                isFailEmail: false,
                sentId:action.sendId,
                summary:state.summary            
            };

        case "FAIL_INVITE_EMAIL":
            return {
                result: state.result,
                isLoading: false,
                isEmailSent: false,
                isFailEmail: true,
                sentId:0,
                summary:state.summary
            };
            case "INVITE_SUMMARY":
            return {
                result: state.result,
                isLoading: true,
                isEmailSent: false,
                isFailEmail: false,
            sentId:state.sentId,
            summary:action.summary
            }
        default:
            const exhaustiveCheck: never = action;
    }
    return state || unloadedState;
};
