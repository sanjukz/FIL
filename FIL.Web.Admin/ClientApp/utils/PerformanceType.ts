export const performanceTypeModel = [
  {
    index: 0,
    performanceTypeId: 1,
    onlineEventTypeId: 1,
    name: 'Pre-recorded video',
    isChecked: true,
    description: ''
  },
  {
    index: 1,
    performanceTypeId: 1,
    onlineEventTypeId: 2,
    name: 'Live performance',
    isChecked: false,
    description: ''
  },
  {
    index: 2,
    performanceTypeId: 2,
    onlineEventTypeId: 1,
    name: 'Pre-recorded video',
    isChecked: true,
    description: ''
  },
  {
    index: 3,
    performanceTypeId: 2,
    onlineEventTypeId: 4,
    name: 'Live performance with artists collocated',
    isChecked: false,
    description: ''
  },
  {
    index: 4,
    performanceTypeId: 2,
    onlineEventTypeId: 8,
    name: 'Live performance of group members, each performing sequentially',
    isChecked: false,
    description: ''
  }
]

export const getFlagStatus = (currentPerformanceType, onlineEventTypeId) => {
  if (onlineEventTypeId == 1) {
    return currentPerformanceType.onlineEventTypeId == 1
  } else if (onlineEventTypeId == 2) {
    return currentPerformanceType.onlineEventTypeId == 2
  } else if (onlineEventTypeId == 3) {
    return currentPerformanceType.onlineEventTypeId == 1 || currentPerformanceType.onlineEventTypeId == 2
  } else if (onlineEventTypeId == 4) {
    return currentPerformanceType.onlineEventTypeId == 4
  } else if (onlineEventTypeId == 8) {
    return currentPerformanceType.onlineEventTypeId == 8
  } else if (onlineEventTypeId == 5) {
    return currentPerformanceType.onlineEventTypeId == 1 || currentPerformanceType.onlineEventTypeId == 4
  } else if (onlineEventTypeId == 9) {
    return currentPerformanceType.onlineEventTypeId == 1 || currentPerformanceType.onlineEventTypeId == 8
  } else if (onlineEventTypeId == 13) {
    return currentPerformanceType.onlineEventTypeId == 1 || currentPerformanceType.onlineEventTypeId == 4 || currentPerformanceType.onlineEventTypeId == 8
  } else if (onlineEventTypeId == 12) {
    return currentPerformanceType.onlineEventTypeId == 4 || currentPerformanceType.onlineEventTypeId == 8
  }
}
