#
# Generated Makefile - do not edit!
#
# Edit the Makefile in the project folder instead (../Makefile). Each target
# has a -pre and a -post target defined where you can add customized code.
#
# This makefile implements configuration specific macros and targets.


# Include project Makefile
ifeq "${IGNORE_LOCAL}" "TRUE"
# do not include local makefile. User is passing all local related variables already
else
include Makefile
# Include makefile containing local settings
ifeq "$(wildcard nbproject/Makefile-local-default.mk)" "nbproject/Makefile-local-default.mk"
include nbproject/Makefile-local-default.mk
endif
endif

# Environment
MKDIR=gnumkdir -p
RM=rm -f 
MV=mv 
CP=cp 

# Macros
CND_CONF=default
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
IMAGE_TYPE=debug
OUTPUT_SUFFIX=elf
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=${DISTDIR}/PRJ05_-_ECU2CAN.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
else
IMAGE_TYPE=production
OUTPUT_SUFFIX=hex
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=${DISTDIR}/PRJ05_-_ECU2CAN.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
endif

ifeq ($(COMPARE_BUILD), true)
COMPARISON_BUILD=-mafrlcsj
else
COMPARISON_BUILD=
endif

# Object Directory
OBJECTDIR=build/${CND_CONF}/${IMAGE_TYPE}

# Distribution Directory
DISTDIR=dist/${CND_CONF}/${IMAGE_TYPE}

# Source Files Quoted if spaced
SOURCEFILES_QUOTED_IF_SPACED=canopen/CO_CANDRV.c canopen/CO_COMM.c canopen/CO_DEV.c canopen/CO_dict.c canopen/CO_MAIN.c canopen/CO_MEMIO.c canopen/CO_NMT.c canopen/CO_NMTE.c canopen/CO_PDO.c canopen/CO_PDO1.c canopen/CO_PDO2.c canopen/CO_PDO3.c canopen/CO_PDO4.c canopen/CO_SDO1.c canopen/CO_SYNC.c canopen/CO_TOOLS.c canopen/DemoObj.c canopen/exttst.c main.c rs232.c system.c dlog.c timers.c canopen.c eeprom.c

# Object Files Quoted if spaced
OBJECTFILES_QUOTED_IF_SPACED=${OBJECTDIR}/canopen/CO_CANDRV.p1 ${OBJECTDIR}/canopen/CO_COMM.p1 ${OBJECTDIR}/canopen/CO_DEV.p1 ${OBJECTDIR}/canopen/CO_dict.p1 ${OBJECTDIR}/canopen/CO_MAIN.p1 ${OBJECTDIR}/canopen/CO_MEMIO.p1 ${OBJECTDIR}/canopen/CO_NMT.p1 ${OBJECTDIR}/canopen/CO_NMTE.p1 ${OBJECTDIR}/canopen/CO_PDO.p1 ${OBJECTDIR}/canopen/CO_PDO1.p1 ${OBJECTDIR}/canopen/CO_PDO2.p1 ${OBJECTDIR}/canopen/CO_PDO3.p1 ${OBJECTDIR}/canopen/CO_PDO4.p1 ${OBJECTDIR}/canopen/CO_SDO1.p1 ${OBJECTDIR}/canopen/CO_SYNC.p1 ${OBJECTDIR}/canopen/CO_TOOLS.p1 ${OBJECTDIR}/canopen/DemoObj.p1 ${OBJECTDIR}/canopen/exttst.p1 ${OBJECTDIR}/main.p1 ${OBJECTDIR}/rs232.p1 ${OBJECTDIR}/system.p1 ${OBJECTDIR}/dlog.p1 ${OBJECTDIR}/timers.p1 ${OBJECTDIR}/canopen.p1 ${OBJECTDIR}/eeprom.p1
POSSIBLE_DEPFILES=${OBJECTDIR}/canopen/CO_CANDRV.p1.d ${OBJECTDIR}/canopen/CO_COMM.p1.d ${OBJECTDIR}/canopen/CO_DEV.p1.d ${OBJECTDIR}/canopen/CO_dict.p1.d ${OBJECTDIR}/canopen/CO_MAIN.p1.d ${OBJECTDIR}/canopen/CO_MEMIO.p1.d ${OBJECTDIR}/canopen/CO_NMT.p1.d ${OBJECTDIR}/canopen/CO_NMTE.p1.d ${OBJECTDIR}/canopen/CO_PDO.p1.d ${OBJECTDIR}/canopen/CO_PDO1.p1.d ${OBJECTDIR}/canopen/CO_PDO2.p1.d ${OBJECTDIR}/canopen/CO_PDO3.p1.d ${OBJECTDIR}/canopen/CO_PDO4.p1.d ${OBJECTDIR}/canopen/CO_SDO1.p1.d ${OBJECTDIR}/canopen/CO_SYNC.p1.d ${OBJECTDIR}/canopen/CO_TOOLS.p1.d ${OBJECTDIR}/canopen/DemoObj.p1.d ${OBJECTDIR}/canopen/exttst.p1.d ${OBJECTDIR}/main.p1.d ${OBJECTDIR}/rs232.p1.d ${OBJECTDIR}/system.p1.d ${OBJECTDIR}/dlog.p1.d ${OBJECTDIR}/timers.p1.d ${OBJECTDIR}/canopen.p1.d ${OBJECTDIR}/eeprom.p1.d

# Object Files
OBJECTFILES=${OBJECTDIR}/canopen/CO_CANDRV.p1 ${OBJECTDIR}/canopen/CO_COMM.p1 ${OBJECTDIR}/canopen/CO_DEV.p1 ${OBJECTDIR}/canopen/CO_dict.p1 ${OBJECTDIR}/canopen/CO_MAIN.p1 ${OBJECTDIR}/canopen/CO_MEMIO.p1 ${OBJECTDIR}/canopen/CO_NMT.p1 ${OBJECTDIR}/canopen/CO_NMTE.p1 ${OBJECTDIR}/canopen/CO_PDO.p1 ${OBJECTDIR}/canopen/CO_PDO1.p1 ${OBJECTDIR}/canopen/CO_PDO2.p1 ${OBJECTDIR}/canopen/CO_PDO3.p1 ${OBJECTDIR}/canopen/CO_PDO4.p1 ${OBJECTDIR}/canopen/CO_SDO1.p1 ${OBJECTDIR}/canopen/CO_SYNC.p1 ${OBJECTDIR}/canopen/CO_TOOLS.p1 ${OBJECTDIR}/canopen/DemoObj.p1 ${OBJECTDIR}/canopen/exttst.p1 ${OBJECTDIR}/main.p1 ${OBJECTDIR}/rs232.p1 ${OBJECTDIR}/system.p1 ${OBJECTDIR}/dlog.p1 ${OBJECTDIR}/timers.p1 ${OBJECTDIR}/canopen.p1 ${OBJECTDIR}/eeprom.p1

# Source Files
SOURCEFILES=canopen/CO_CANDRV.c canopen/CO_COMM.c canopen/CO_DEV.c canopen/CO_dict.c canopen/CO_MAIN.c canopen/CO_MEMIO.c canopen/CO_NMT.c canopen/CO_NMTE.c canopen/CO_PDO.c canopen/CO_PDO1.c canopen/CO_PDO2.c canopen/CO_PDO3.c canopen/CO_PDO4.c canopen/CO_SDO1.c canopen/CO_SYNC.c canopen/CO_TOOLS.c canopen/DemoObj.c canopen/exttst.c main.c rs232.c system.c dlog.c timers.c canopen.c eeprom.c



CFLAGS=
ASFLAGS=
LDLIBSOPTIONS=

############# Tool locations ##########################################
# If you copy a project from one host to another, the path where the  #
# compiler is installed may be different.                             #
# If you open this project with MPLAB X in the new host, this         #
# makefile will be regenerated and the paths will be corrected.       #
#######################################################################
# fixDeps replaces a bunch of sed/cat/printf statements that slow down the build
FIXDEPS=fixDeps

.build-conf:  ${BUILD_SUBPROJECTS}
ifneq ($(INFORMATION_MESSAGE), )
	@echo $(INFORMATION_MESSAGE)
endif
	${MAKE}  -f nbproject/Makefile-default.mk ${DISTDIR}/PRJ05_-_ECU2CAN.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}

MP_PROCESSOR_OPTION=18F25K80
# ------------------------------------------------------------------------------------
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/canopen/CO_CANDRV.p1: canopen/CO_CANDRV.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_CANDRV.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_CANDRV.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_CANDRV.p1 canopen/CO_CANDRV.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_CANDRV.d ${OBJECTDIR}/canopen/CO_CANDRV.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_CANDRV.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_COMM.p1: canopen/CO_COMM.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_COMM.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_COMM.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_COMM.p1 canopen/CO_COMM.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_COMM.d ${OBJECTDIR}/canopen/CO_COMM.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_COMM.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_DEV.p1: canopen/CO_DEV.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_DEV.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_DEV.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_DEV.p1 canopen/CO_DEV.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_DEV.d ${OBJECTDIR}/canopen/CO_DEV.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_DEV.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_dict.p1: canopen/CO_dict.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_dict.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_dict.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_dict.p1 canopen/CO_dict.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_dict.d ${OBJECTDIR}/canopen/CO_dict.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_dict.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_MAIN.p1: canopen/CO_MAIN.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_MAIN.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_MAIN.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_MAIN.p1 canopen/CO_MAIN.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_MAIN.d ${OBJECTDIR}/canopen/CO_MAIN.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_MAIN.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_MEMIO.p1: canopen/CO_MEMIO.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_MEMIO.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_MEMIO.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_MEMIO.p1 canopen/CO_MEMIO.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_MEMIO.d ${OBJECTDIR}/canopen/CO_MEMIO.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_MEMIO.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_NMT.p1: canopen/CO_NMT.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_NMT.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_NMT.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_NMT.p1 canopen/CO_NMT.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_NMT.d ${OBJECTDIR}/canopen/CO_NMT.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_NMT.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_NMTE.p1: canopen/CO_NMTE.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_NMTE.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_NMTE.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_NMTE.p1 canopen/CO_NMTE.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_NMTE.d ${OBJECTDIR}/canopen/CO_NMTE.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_NMTE.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_PDO.p1: canopen/CO_PDO.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_PDO.p1 canopen/CO_PDO.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_PDO.d ${OBJECTDIR}/canopen/CO_PDO.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_PDO.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_PDO1.p1: canopen/CO_PDO1.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO1.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO1.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_PDO1.p1 canopen/CO_PDO1.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_PDO1.d ${OBJECTDIR}/canopen/CO_PDO1.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_PDO1.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_PDO2.p1: canopen/CO_PDO2.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO2.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO2.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_PDO2.p1 canopen/CO_PDO2.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_PDO2.d ${OBJECTDIR}/canopen/CO_PDO2.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_PDO2.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_PDO3.p1: canopen/CO_PDO3.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO3.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO3.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_PDO3.p1 canopen/CO_PDO3.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_PDO3.d ${OBJECTDIR}/canopen/CO_PDO3.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_PDO3.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_PDO4.p1: canopen/CO_PDO4.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO4.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO4.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_PDO4.p1 canopen/CO_PDO4.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_PDO4.d ${OBJECTDIR}/canopen/CO_PDO4.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_PDO4.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_SDO1.p1: canopen/CO_SDO1.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_SDO1.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_SDO1.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_SDO1.p1 canopen/CO_SDO1.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_SDO1.d ${OBJECTDIR}/canopen/CO_SDO1.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_SDO1.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_SYNC.p1: canopen/CO_SYNC.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_SYNC.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_SYNC.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_SYNC.p1 canopen/CO_SYNC.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_SYNC.d ${OBJECTDIR}/canopen/CO_SYNC.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_SYNC.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_TOOLS.p1: canopen/CO_TOOLS.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_TOOLS.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_TOOLS.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_TOOLS.p1 canopen/CO_TOOLS.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_TOOLS.d ${OBJECTDIR}/canopen/CO_TOOLS.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_TOOLS.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/DemoObj.p1: canopen/DemoObj.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/DemoObj.p1.d 
	@${RM} ${OBJECTDIR}/canopen/DemoObj.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/DemoObj.p1 canopen/DemoObj.c 
	@-${MV} ${OBJECTDIR}/canopen/DemoObj.d ${OBJECTDIR}/canopen/DemoObj.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/DemoObj.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/exttst.p1: canopen/exttst.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/exttst.p1.d 
	@${RM} ${OBJECTDIR}/canopen/exttst.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/exttst.p1 canopen/exttst.c 
	@-${MV} ${OBJECTDIR}/canopen/exttst.d ${OBJECTDIR}/canopen/exttst.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/exttst.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/main.p1: main.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/main.p1.d 
	@${RM} ${OBJECTDIR}/main.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/main.p1 main.c 
	@-${MV} ${OBJECTDIR}/main.d ${OBJECTDIR}/main.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/main.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/rs232.p1: rs232.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/rs232.p1.d 
	@${RM} ${OBJECTDIR}/rs232.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/rs232.p1 rs232.c 
	@-${MV} ${OBJECTDIR}/rs232.d ${OBJECTDIR}/rs232.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/rs232.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/system.p1: system.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/system.p1.d 
	@${RM} ${OBJECTDIR}/system.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/system.p1 system.c 
	@-${MV} ${OBJECTDIR}/system.d ${OBJECTDIR}/system.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/system.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/dlog.p1: dlog.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/dlog.p1.d 
	@${RM} ${OBJECTDIR}/dlog.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/dlog.p1 dlog.c 
	@-${MV} ${OBJECTDIR}/dlog.d ${OBJECTDIR}/dlog.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/dlog.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/timers.p1: timers.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/timers.p1.d 
	@${RM} ${OBJECTDIR}/timers.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/timers.p1 timers.c 
	@-${MV} ${OBJECTDIR}/timers.d ${OBJECTDIR}/timers.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/timers.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen.p1: canopen.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/canopen.p1.d 
	@${RM} ${OBJECTDIR}/canopen.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen.p1 canopen.c 
	@-${MV} ${OBJECTDIR}/canopen.d ${OBJECTDIR}/canopen.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/eeprom.p1: eeprom.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/eeprom.p1.d 
	@${RM} ${OBJECTDIR}/eeprom.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c  -D__DEBUG=1  -mdebugger=icd3   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/eeprom.p1 eeprom.c 
	@-${MV} ${OBJECTDIR}/eeprom.d ${OBJECTDIR}/eeprom.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/eeprom.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
else
${OBJECTDIR}/canopen/CO_CANDRV.p1: canopen/CO_CANDRV.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_CANDRV.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_CANDRV.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_CANDRV.p1 canopen/CO_CANDRV.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_CANDRV.d ${OBJECTDIR}/canopen/CO_CANDRV.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_CANDRV.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_COMM.p1: canopen/CO_COMM.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_COMM.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_COMM.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_COMM.p1 canopen/CO_COMM.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_COMM.d ${OBJECTDIR}/canopen/CO_COMM.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_COMM.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_DEV.p1: canopen/CO_DEV.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_DEV.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_DEV.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_DEV.p1 canopen/CO_DEV.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_DEV.d ${OBJECTDIR}/canopen/CO_DEV.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_DEV.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_dict.p1: canopen/CO_dict.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_dict.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_dict.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_dict.p1 canopen/CO_dict.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_dict.d ${OBJECTDIR}/canopen/CO_dict.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_dict.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_MAIN.p1: canopen/CO_MAIN.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_MAIN.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_MAIN.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_MAIN.p1 canopen/CO_MAIN.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_MAIN.d ${OBJECTDIR}/canopen/CO_MAIN.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_MAIN.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_MEMIO.p1: canopen/CO_MEMIO.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_MEMIO.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_MEMIO.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_MEMIO.p1 canopen/CO_MEMIO.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_MEMIO.d ${OBJECTDIR}/canopen/CO_MEMIO.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_MEMIO.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_NMT.p1: canopen/CO_NMT.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_NMT.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_NMT.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_NMT.p1 canopen/CO_NMT.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_NMT.d ${OBJECTDIR}/canopen/CO_NMT.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_NMT.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_NMTE.p1: canopen/CO_NMTE.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_NMTE.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_NMTE.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_NMTE.p1 canopen/CO_NMTE.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_NMTE.d ${OBJECTDIR}/canopen/CO_NMTE.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_NMTE.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_PDO.p1: canopen/CO_PDO.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_PDO.p1 canopen/CO_PDO.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_PDO.d ${OBJECTDIR}/canopen/CO_PDO.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_PDO.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_PDO1.p1: canopen/CO_PDO1.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO1.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO1.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_PDO1.p1 canopen/CO_PDO1.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_PDO1.d ${OBJECTDIR}/canopen/CO_PDO1.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_PDO1.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_PDO2.p1: canopen/CO_PDO2.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO2.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO2.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_PDO2.p1 canopen/CO_PDO2.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_PDO2.d ${OBJECTDIR}/canopen/CO_PDO2.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_PDO2.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_PDO3.p1: canopen/CO_PDO3.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO3.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO3.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_PDO3.p1 canopen/CO_PDO3.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_PDO3.d ${OBJECTDIR}/canopen/CO_PDO3.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_PDO3.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_PDO4.p1: canopen/CO_PDO4.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO4.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_PDO4.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_PDO4.p1 canopen/CO_PDO4.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_PDO4.d ${OBJECTDIR}/canopen/CO_PDO4.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_PDO4.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_SDO1.p1: canopen/CO_SDO1.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_SDO1.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_SDO1.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_SDO1.p1 canopen/CO_SDO1.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_SDO1.d ${OBJECTDIR}/canopen/CO_SDO1.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_SDO1.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_SYNC.p1: canopen/CO_SYNC.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_SYNC.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_SYNC.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_SYNC.p1 canopen/CO_SYNC.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_SYNC.d ${OBJECTDIR}/canopen/CO_SYNC.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_SYNC.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/CO_TOOLS.p1: canopen/CO_TOOLS.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/CO_TOOLS.p1.d 
	@${RM} ${OBJECTDIR}/canopen/CO_TOOLS.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/CO_TOOLS.p1 canopen/CO_TOOLS.c 
	@-${MV} ${OBJECTDIR}/canopen/CO_TOOLS.d ${OBJECTDIR}/canopen/CO_TOOLS.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/CO_TOOLS.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/DemoObj.p1: canopen/DemoObj.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/DemoObj.p1.d 
	@${RM} ${OBJECTDIR}/canopen/DemoObj.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/DemoObj.p1 canopen/DemoObj.c 
	@-${MV} ${OBJECTDIR}/canopen/DemoObj.d ${OBJECTDIR}/canopen/DemoObj.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/DemoObj.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen/exttst.p1: canopen/exttst.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}/canopen" 
	@${RM} ${OBJECTDIR}/canopen/exttst.p1.d 
	@${RM} ${OBJECTDIR}/canopen/exttst.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen/exttst.p1 canopen/exttst.c 
	@-${MV} ${OBJECTDIR}/canopen/exttst.d ${OBJECTDIR}/canopen/exttst.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen/exttst.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/main.p1: main.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/main.p1.d 
	@${RM} ${OBJECTDIR}/main.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/main.p1 main.c 
	@-${MV} ${OBJECTDIR}/main.d ${OBJECTDIR}/main.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/main.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/rs232.p1: rs232.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/rs232.p1.d 
	@${RM} ${OBJECTDIR}/rs232.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/rs232.p1 rs232.c 
	@-${MV} ${OBJECTDIR}/rs232.d ${OBJECTDIR}/rs232.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/rs232.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/system.p1: system.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/system.p1.d 
	@${RM} ${OBJECTDIR}/system.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/system.p1 system.c 
	@-${MV} ${OBJECTDIR}/system.d ${OBJECTDIR}/system.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/system.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/dlog.p1: dlog.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/dlog.p1.d 
	@${RM} ${OBJECTDIR}/dlog.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/dlog.p1 dlog.c 
	@-${MV} ${OBJECTDIR}/dlog.d ${OBJECTDIR}/dlog.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/dlog.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/timers.p1: timers.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/timers.p1.d 
	@${RM} ${OBJECTDIR}/timers.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/timers.p1 timers.c 
	@-${MV} ${OBJECTDIR}/timers.d ${OBJECTDIR}/timers.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/timers.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/canopen.p1: canopen.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/canopen.p1.d 
	@${RM} ${OBJECTDIR}/canopen.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/canopen.p1 canopen.c 
	@-${MV} ${OBJECTDIR}/canopen.d ${OBJECTDIR}/canopen.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/canopen.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/eeprom.p1: eeprom.c  nbproject/Makefile-${CND_CONF}.mk 
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/eeprom.p1.d 
	@${RM} ${OBJECTDIR}/eeprom.p1 
	${MP_CC} $(MP_EXTRA_CC_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -c   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -DXPRJ_default=$(CND_CONF)  -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits $(COMPARISON_BUILD)  -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     -o ${OBJECTDIR}/eeprom.p1 eeprom.c 
	@-${MV} ${OBJECTDIR}/eeprom.d ${OBJECTDIR}/eeprom.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/eeprom.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: assemble
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: assembleWithPreprocess
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${DISTDIR}/PRJ05_-_ECU2CAN.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk    
	@${MKDIR} ${DISTDIR} 
	${MP_CC} $(MP_EXTRA_LD_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -Wl,-Map=${DISTDIR}/PRJ05_-_ECU2CAN.X.${IMAGE_TYPE}.map  -D__DEBUG=1  -mdebugger=icd3  -DXPRJ_default=$(CND_CONF)  -Wl,--defsym=__MPLAB_BUILD=1   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto        $(COMPARISON_BUILD) -Wl,--memorysummary,${DISTDIR}/memoryfile.xml -o ${DISTDIR}/PRJ05_-_ECU2CAN.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}     
	@${RM} ${DISTDIR}/PRJ05_-_ECU2CAN.X.${IMAGE_TYPE}.hex 
	
	
else
${DISTDIR}/PRJ05_-_ECU2CAN.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk   
	@${MKDIR} ${DISTDIR} 
	${MP_CC} $(MP_EXTRA_LD_PRE) -mcpu=$(MP_PROCESSOR_OPTION) -Wl,-Map=${DISTDIR}/PRJ05_-_ECU2CAN.X.${IMAGE_TYPE}.map  -DXPRJ_default=$(CND_CONF)  -Wl,--defsym=__MPLAB_BUILD=1   -mdfp="${DFP_DIR}/xc8"  -fno-short-double -fno-short-float -memi=wordwrite -O0 -fasmfile -maddrqual=ignore -xassembler-with-cpp -mwarn=-3 -Wa,-a -msummary=-psect,-class,+mem,-hex,-file  -ginhx32 -Wl,--data-init -mno-keep-startup -mno-download -mdefault-config-bits -std=c90 -gdwarf-3 -mstack=compiled:auto:auto:auto     $(COMPARISON_BUILD) -Wl,--memorysummary,${DISTDIR}/memoryfile.xml -o ${DISTDIR}/PRJ05_-_ECU2CAN.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}     
	
	
endif


# Subprojects
.build-subprojects:


# Subprojects
.clean-subprojects:

# Clean Targets
.clean-conf: ${CLEAN_SUBPROJECTS}
	${RM} -r ${OBJECTDIR}
	${RM} -r ${DISTDIR}

# Enable dependency checking
.dep.inc: .depcheck-impl

DEPFILES=$(wildcard ${POSSIBLE_DEPFILES})
ifneq (${DEPFILES},)
include ${DEPFILES}
endif
