
import { StepViewModel, StepModel } from "../models/CreateEventV1/StepViewModel";
import { REQUIRED_SUBMISSION_STEP_ARRAY, REQUIRED_FINANCE_STEP_ARRAY } from './Constant/Step';

export const getCurrentStep = (stepModel: StepViewModel) => {
  let currentStep = stepModel.stepModel.filter((val: StepModel) => {
    return val.stepId == stepModel.currentStep
  });
  if (currentStep.length > 0) {
    return currentStep[0]
  } else {
    return;
  }
}

export const getCurrentStepBySlug = (stepModel: StepViewModel, slug: string) => {
  let currentStep = stepModel.stepModel.filter((val: StepModel) => {
    return val.slug == slug
  });
  if (currentStep.length > 0) {
    return currentStep[0]
  } else {
    return;
  }
}

export const getCurrentStepByStepId = (stepModel: StepViewModel, stepId: number) => {
  return stepModel.stepModel.filter((val: StepModel) => {
    return val.stepId == stepId
  })[0];
}

export const getCurrentStepIndex = (stepModel, currentStateStep) => {
  let currentStep = 0;
  stepModel.forEach((val, index) => {
    if (val.stepId == currentStateStep) {
      currentStep = index;
      return;
    }
  })
  return currentStep;
}

export const getStepStatus = (props, val, currentStep) => {
  if (val.stepId == currentStep) {
    return false;
  } else if (props.stepModel.completedStep.indexOf(val.stepId) > -1) {
    return true;
  } else {
    return false;
  }
}


export const getStepDisable = (stepArray, completedStep) => {
  let isDisable = false;
  stepArray.forEach((val) => {
    if (completedStep.indexOf(val) == -1) {
      isDisable = true;
      return;
    }
  });
  return isDisable
}


export const getDisableStatus = (completedStep, stateModel, currentStepModel, slug) => {
  let isDisable = false;
  if (!slug && currentStepModel.stepId > 1) {
    return true;
  } else if (slug && currentStepModel.stepId == 9) {
    return getStepDisable(REQUIRED_FINANCE_STEP_ARRAY, completedStep);
  } else if (slug && currentStepModel.stepId == 12) {
    return getStepDisable(REQUIRED_SUBMISSION_STEP_ARRAY, completedStep);
  } else {
    return isDisable;
  }
}
